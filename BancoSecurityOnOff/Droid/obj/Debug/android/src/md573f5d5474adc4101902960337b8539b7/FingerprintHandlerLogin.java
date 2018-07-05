package md573f5d5474adc4101902960337b8539b7;


public class FingerprintHandlerLogin
	extends android.hardware.fingerprint.FingerprintManager.AuthenticationCallback
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onAuthenticationFailed:()V:GetOnAuthenticationFailedHandler\n" +
			"n_onAuthenticationSucceeded:(Landroid/hardware/fingerprint/FingerprintManager$AuthenticationResult;)V:GetOnAuthenticationSucceeded_Landroid_hardware_fingerprint_FingerprintManager_AuthenticationResult_Handler\n" +
			"";
		mono.android.Runtime.register ("BancoSecurityOnOff.Droid.WebServiceSecurity.HelperFinger.FingerprintHandlerLogin, BancoSecurityOnOff.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", FingerprintHandlerLogin.class, __md_methods);
	}


	public FingerprintHandlerLogin ()
	{
		super ();
		if (getClass () == FingerprintHandlerLogin.class)
			mono.android.TypeManager.Activate ("BancoSecurityOnOff.Droid.WebServiceSecurity.HelperFinger.FingerprintHandlerLogin, BancoSecurityOnOff.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public FingerprintHandlerLogin (android.content.Context p0)
	{
		super ();
		if (getClass () == FingerprintHandlerLogin.class)
			mono.android.TypeManager.Activate ("BancoSecurityOnOff.Droid.WebServiceSecurity.HelperFinger.FingerprintHandlerLogin, BancoSecurityOnOff.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}


	public void onAuthenticationFailed ()
	{
		n_onAuthenticationFailed ();
	}

	private native void n_onAuthenticationFailed ();


	public void onAuthenticationSucceeded (android.hardware.fingerprint.FingerprintManager.AuthenticationResult p0)
	{
		n_onAuthenticationSucceeded (p0);
	}

	private native void n_onAuthenticationSucceeded (android.hardware.fingerprint.FingerprintManager.AuthenticationResult p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
