#!groovy
node('MacOS') {

/** ************************************************************/
/*                 Parámetros del Proyecto                    */
/** ************************************************************/

    def projectEmail = "jreyes@tinet.cl"

    def urlSonarHost = "https://sonar-soporte.tinet.cl"
    def sonarToken = "80e395d9b08dd7d6aeed56c19a8aa14e3c075504"
    def nombreSolucion = ""

 
/** **************************************************************/
/*                Etapa 1 - Actualizacion De Fuentes            */
/** **************************************************************/
    stage 'Checkout'
    checkout scm
    def branchNameFile = "${env.JOB_NAME.replaceFirst('.+/', '').replaceFirst('%2F', '_')}"
    echo 'el branchNameFile es ' + branchNameFile
    sh "git rev-parse HEAD > ${branchNameFile}"
    def commit = readFile("${branchNameFile}").trim()
    echo 'el commit es ' + commit

/** **************************************************************/
/*                Etapa 2 - Construcción de artefactos           */
/** **************************************************************/
    stage 'Compilar Proyectos'



       compilar("BancoSecurityOnOff", urlSonarHost, sonarToken, projectEmail, branchNameFile) 

  
    stage 'Analisis Estatico'
       analisis_estatico("BancoSecurityOnOff", sonarToken, projectEmail, branchNameFile) 


/*
    stage 'Generar Artefactos'

        dir("BancoSecurityOnOff") {



            sh """
                   /Library/Frameworks/Mono.framework/Versions/Current/Commands/nuget restore
               """

            sh """
                /Library/Frameworks/Mono.framework/Versions/Current/Commands/msbuild BancoSecurityOnOff.sln /t:BancoSecurityOnOff_iOS /p:Configuration=Release /p:BuildIpa=true  /p:Platform=iPhone /p:IpaPackageDir=./Builds /verbosity:detailed
               """

            sh "pwd"

            
            sh "ls -la"

            sh "ls -la ./Builds"
        }
        */

}




def compilar(nombreSolucion, urlSonarHost, sonarToken, projectEmail, branchNameFile) {
    

        echo "Solución a compilar: ${nombreSolucion}"

        dir(nombreSolucion) {



            try {
                     sh """
                        /Library/Frameworks/Mono.framework/Versions/Current/Commands/mono /Users/pipeline/herramientas/sonar-scanner-msbuild-4/SonarQube.Scanner.MSBuild.exe begin /k:${nombreSolucion}-Mobile-${branchNameFile} /d:sonar.host.url=${urlSonarHost} /d:sonar.login=${sonarToken}
                        """

                    sh "pwd"

                    sh "/Library/Frameworks/Mono.framework/Versions/Current/Commands/msbuild /t:Rebuild"

                        
                    sh "zip -r BancoSecurityOnOff.zip ./BancoSecurityOnOff/bin"
                    sh "zip -r Droid.zip ./Droid/bin"
                    sh "zip -r iOS.zip ./iOS/bin"

                    
                    sh "ls -la"


                    archive "BancoSecurityOnOff.zip"
                    archive "Droid.zip"
                    archive "iOS.zip"
            }
            catch (Exception e) {
                echo "Error al ejecutar la compilación"
                echo e.getMessage()
                send_compilacion_email "${projectEmail}", "MSBuild", "${env.BUILD_URL}"
                error "Error al ejecutar la compilación"
            }


        }


}


def analisis_estatico (nombreSolucion, sonarToken, projectEmail, branchNameFile) {

            dir(nombreSolucion) {


            try {


                sh """
                    /Library/Frameworks/Mono.framework/Versions/Current/Commands/mono /Users/pipeline/herramientas/sonar-scanner-msbuild-4/SonarQube.Scanner.MSBuild.exe end /d:sonar.login=${sonarToken}
                   """

                send_ejecucion_email "${projectEmail}", "Jenkins", "${env.BUILD_URL}", "https://sonar7.tinet.cl/dashboard?id=${nombreSolucion}-Mobile-${branchNameFile}", "${nombreSolucion}"
            }
            catch (Exception e) {
                echo "Error al ejecutar el análisis estático"
                echo e.getMessage()
                send_static_analysis_email "${projectEmail}", "Sonarqube", "${env.BUILD_URL}"
                error "Error al ejecutar el análisis estático"
            }


        }
}



def send_static_analysis_email(dirs, type, reportURL) {
    mail   \
      to: "${dirs}",
            subject: "[Fallo en Analisis Estatico] -> Proyecto '${env.JOB_NAME}' (#${env.BUILD_NUMBER})",
            body: """\
       El proyecto '${env.JOB_NAME}' (#${env.BUILD_NUMBER}) fallo la ejecucion del analisis estatico
       
       Herramienta: ${type}

       Reporte de la ejecucion:  ${reportURL}

       Build:  ${env.BUILD_URL} 
       """
}


def send_ejecucion_email(dirs, type, reportURL, sonarUrl, nombreSolucion) {
    mail   \
      to: "${dirs}",
            subject: "[Nueva Ejecucion de Pipeline] -> Proyecto '${env.JOB_NAME}' (#${env.BUILD_NUMBER})",
            body: """\
       El proyecto '${env.JOB_NAME}' (#${env.BUILD_NUMBER}) fue ejecutado en el pipeline
       
       Herramienta: ${type}

       Solucion: ${nombreSolucion}

       Reporte de la ejecucion Sonar:  ${sonarUrl}

       Build:  ${env.BUILD_URL} 
       """
}

def send_compilacion_email(dirs, type, reportURL) {
    mail   \
      to: "${dirs}",
            subject: "[Fallo en Compilacion] -> Proyecto '${env.JOB_NAME}' (#${env.BUILD_NUMBER})",
            body: """\
       El proyecto '${env.JOB_NAME}' (#${env.BUILD_NUMBER}) fallo la ejecucion de la compilacion
       
       Herramienta: ${type}

       Reporte de la ejecucion:  ${reportURL}

       Build:  ${env.BUILD_URL} 
       """
}


