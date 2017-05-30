package com.example.batuhan.myapplication2;

import android.Manifest;
import android.annotation.TargetApi;
import android.content.DialogInterface;
import android.content.pm.PackageManager;
import android.os.Build;
import android.support.annotation.NonNull;
import android.support.v4.app.ActivityCompat;
import android.support.v4.content.ContextCompat;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.content.Context;
import android.nfc.NdefMessage;
import android.nfc.NdefRecord;
import android.nfc.NfcAdapter;
import android.nfc.NfcEvent;
import android.telephony.TelephonyManager;
import android.util.Log;
import android.view.View;
import android.widget.TextView;
import android.widget.Toast;

import java.util.Arrays;

import static java.lang.Math.abs;

public class MainActivity extends AppCompatActivity implements NfcAdapter.CreateNdefMessageCallback {
    private static final int MY_PERMISSIONS_REQUEST_READ_PHONE_STATE = 1;
    TelephonyManager telephonyManager;
    TextView textView;
    TextView textView1;

    private NfcAdapter mNfcAdapter;
  // private String imei = "";
    //String imei;
    @Override
    protected void onCreate(Bundle savedInstanceState) {

        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        textView= (TextView) findViewById(R.id.textView);
        textView1= (TextView) findViewById(R.id.textView1);
        int [] dizi=new  int[4];

        telephonyManager= (TelephonyManager) getSystemService(Context.TELEPHONY_SERVICE);
        StringBuilder builder=new StringBuilder();
        String imei=telephonyManager.getDeviceId();

       //try {
         //   TelephonyManager m_telephonyManager = (TelephonyManager) getSystemService(getApplicationContext().TELEPHONY_SERVICE);
           //  imei = m_telephonyManager.getDeviceId();
        //} catch (Exception e) {
        //}

      // String code=   imei.charAt(1)+" "+imei.charAt(2)+"";
      //  String code=   imei.charAt(0)+""+imei.charAt(1)+""+imei.charAt(2)+""+imei.charAt(12)+""+imei.charAt(13)+""+imei.charAt(14)+""+abs(Integer.valueOf(imei.charAt(14))-Integer.valueOf(imei.charAt(0))+"";


        char first =imei.charAt(0);
        char second=imei.charAt(14);
        int firstint=Integer.valueOf(first);
        int secondint=Integer.valueOf(second);
        dizi[0]=firstint;
        dizi[1]=secondint;
       dizi[2]=secondint-firstint;// dizi[2]=secondint-firstint;
        dizi[3]=abs(dizi[2]);
        StringBuilder sb = new StringBuilder(dizi.length);
        for (int i : dizi) {//int diziyi stringe ceviriyorum
           sb.append(i);
        }
        String s = sb.toString(); // 1010
        String code=   imei.charAt(0)+""+imei.charAt(1)+""+imei.charAt(2)+""+imei.charAt(12)+""+imei.charAt(13)+""+imei.charAt(14)+""+dizi[3]+"";
       textView.setText(code);//değiştirilmis imei no
        textView1.setText(imei); //normal imei no
        mNfcAdapter = NfcAdapter.getDefaultAdapter(this);
        if (mNfcAdapter == null) {
            Toast.makeText(this, "NFC is not available", Toast.LENGTH_LONG)
                    .show();
            finish();
            return;
        }
        // Register callback
        mNfcAdapter.setNdefPushMessageCallback(this, this);
    }



    @Override
    public NdefMessage createNdefMessage(NfcEvent event) {
        byte[] textBytes = textView.getText().toString().getBytes();
        NdefRecord textRecord = new NdefRecord(NdefRecord.TNF_MIME_MEDIA,
                "text/plain".getBytes(), new byte[] {}, textBytes);
        return new NdefMessage(new NdefRecord[] { textRecord });
    }
}