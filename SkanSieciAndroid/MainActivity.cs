using Android.App;
using Android.Widget;
using Android.OS;

using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Net.Sockets;
using Xamarin;

namespace SkanSieciAndroid
{
    [Activity(Label = "SkanSieciAndroid", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        List<string> urzadzenia;
        ArrayAdapter<string> adapter;
        string wybranasiec_trim = "192.168.0.";


        Skan skan1;
        Skan skan2;
        Skan skan3;
        Skan skan4;
        Skan skan5;
        Skan skan6;
        Skan skan7;
        Skan skan8;
        Skan skan9;
        Skan skan10;

        Button Skan;
        Button Stop;
        Button Pokaz_IP;
        TextView textview1;
        TextView textview2;
        ListView listView1;
        ProgressBar progressbar;

        Thread thread1 = null;
        Thread thread2 = null;
        Thread thread3 = null;
        Thread thread4 = null;
        Thread thread5 = null;
        Thread thread6 = null;
        Thread thread7 = null;
        Thread thread8 = null;
        Thread thread9 = null;
        Thread thread10 = null;
        Thread glowny = null;



        protected override void OnCreate(Bundle bundle)
        {
            urzadzenia = new List<string>();
            adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, urzadzenia);

            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            Skan = FindViewById<Button>(Resource.Id.button1);
            Stop = FindViewById<Button>(Resource.Id.button2);
            Pokaz_IP = FindViewById<Button>(Resource.Id.button3);
            textview1 = FindViewById<TextView>(Resource.Id.textView1);
            textview2 = FindViewById<TextView>(Resource.Id.textView2);
            listView1 = FindViewById<ListView>(Resource.Id.listView1);
            progressbar = FindViewById<ProgressBar>(Resource.Id.progressBar1);

            Skan.Text = "SKAN";
            Stop.Text = "STOP";
            
            Stop.Enabled = false;
            progressbar.Max = 255;
            progressbar.Progress = 0;

            string wybranasiec = "0.0.0.0";
            IPHostEntry IP = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in IP.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    wybranasiec = ip.ToString();
                }
            }

            string[] temp = wybranasiec.Split('.');
            wybranasiec_trim = temp[0] + "." + temp[1] + "." + temp[2] + ".";

            textview1.Text = wybranasiec_trim;
            textview2.Text = String.Empty;

            Skan.Click += delegate
            {
                skan1 = new Skan(wybranasiec_trim, 0, 24, false);
                skan2 = new Skan(wybranasiec_trim, 25, 49, false);
                skan3 = new Skan(wybranasiec_trim, 50, 74, false);
                skan4 = new Skan(wybranasiec_trim, 75, 99, false);
                skan5 = new Skan(wybranasiec_trim, 100, 124, false);
                skan6 = new Skan(wybranasiec_trim, 125, 149, false);
                skan7 = new Skan(wybranasiec_trim, 150, 174, false);
                skan8 = new Skan(wybranasiec_trim, 175, 199, false);
                skan9 = new Skan(wybranasiec_trim, 200, 224, false);
                skan10 = new Skan(wybranasiec_trim, 225, 255, false);

                glowny = new Thread(new ThreadStart(obsluga));
                glowny.Start();

                thread1 = new Thread(new ThreadStart(skan1.skan));
                thread2 = new Thread(new ThreadStart(skan2.skan));
                thread3 = new Thread(new ThreadStart(skan3.skan));
                thread4 = new Thread(new ThreadStart(skan4.skan));
                thread5 = new Thread(new ThreadStart(skan5.skan));
                thread6 = new Thread(new ThreadStart(skan6.skan));
                thread7 = new Thread(new ThreadStart(skan7.skan));
                thread8 = new Thread(new ThreadStart(skan8.skan));
                thread9 = new Thread(new ThreadStart(skan9.skan));
                thread10 = new Thread(new ThreadStart(skan10.skan));

                thread1.Start();
                thread2.Start();
                thread3.Start();
                thread4.Start();
                thread5.Start();
                thread6.Start();
                thread7.Start();
                thread8.Start();
                thread9.Start();
                thread10.Start();
               
                urzadzenia.Clear();
                adapter.NotifyDataSetChanged();
                adapter.Clear();
                Stop.Enabled = true;
                Skan.Enabled = false;
                listView1.Adapter = adapter;

                RunOnUiThread(() =>
                {
                    textview2.Text = "Pracuje...";
                });
            };

            Stop.Click += delegate
            {
                if (thread1.IsAlive)
                {
                    skan1.Stop = true;
                    thread1.Abort();
                }
                if (thread2.IsAlive)
                {
                    skan2.Stop = true;
                    thread2.Abort();
                }
                if (thread3.IsAlive)
                {
                    skan3.Stop = true;
                    thread3.Abort();
                }
                if (thread4.IsAlive)
                {
                    skan4.Stop = true;
                    thread4.Abort();
                }
                if (thread5.IsAlive)
                {
                    skan5.Stop = true;
                    thread5.Abort();
                }
                if (thread6.IsAlive)
                {
                    skan6.Stop = true;
                    thread6.Abort();
                }
                if (thread7.IsAlive)
                {
                    skan7.Stop = true;
                    thread7.Abort();
                }
                if (thread8.IsAlive)
                {
                    skan8.Stop = true;
                    thread8.Abort();
                }
                if (thread9.IsAlive)
                {
                    skan9.Stop = true;
                    thread9.Abort();
                }
                if (thread10.IsAlive)
                {
                    skan10.Stop = true;
                    thread10.Abort();
                }

                if (glowny.IsAlive)
                {
                    glowny.Abort();
                }
                textview2.Text = "Zatrzymany";
                textview1.Text = wybranasiec_trim + "0";
                Skan.Enabled = true;
                Stop.Enabled = false;
            };

            Pokaz_IP.Click += delegate
            {
                new AlertDialog.Builder(this).SetMessage("Twoje IP: "+wybranasiec).Show();
            };
        }

        public override void OnBackPressed()
        {

            var builder = new AlertDialog.Builder(this);
            builder.SetTitle("Koniec");
            builder.SetMessage("Czy na pewno chcesz wyjść?");
            builder.SetNegativeButton("Nie", (s, e) => { });
            builder.SetPositiveButton("Tak", (s, e) => { Process.KillProcess(Process.MyPid()); });
            builder.Create().Show();

        }

        public void obsluga()
        {
            int postep = 0;
            bool koncz1 = false;
            bool koncz2 = false;
            bool koncz3 = false;
            bool koncz4 = false;
            bool koncz5 = false;
            bool koncz6 = false;
            bool koncz7 = false;
            bool koncz8 = false;
            bool koncz9 = false;
            bool koncz10 = false;
            do
            {
                if (skan1.Czy_koniec == true && koncz1 == false)
                {
                    urzadzenia.AddRange(skan1.ZnalezioneAdresy);
                    koncz1 = true;
                }

                if (skan2.Czy_koniec == true && koncz2 == false)
                {
                    urzadzenia.AddRange(skan2.ZnalezioneAdresy);
                    koncz2 = true;
                }

                if (skan3.Czy_koniec == true && koncz3 == false)
                {
                    urzadzenia.AddRange(skan3.ZnalezioneAdresy);
                    koncz3 = true;
                }

                if (skan4.Czy_koniec == true && koncz4 == false)
                {
                    urzadzenia.AddRange(skan4.ZnalezioneAdresy);
                    koncz4 = true;
                }

                if (skan5.Czy_koniec == true && koncz5 == false)
                {
                    urzadzenia.AddRange(skan5.ZnalezioneAdresy); ;
                    koncz5 = true;
                }

                if (skan6.Czy_koniec == true && koncz6 == false)
                {
                    urzadzenia.AddRange(skan6.ZnalezioneAdresy); ;
                    koncz6 = true;
                }

                if (skan7.Czy_koniec == true && koncz7 == false)
                {
                    urzadzenia.AddRange(skan7.ZnalezioneAdresy); ;
                    koncz7 = true;
                }

                if (skan8.Czy_koniec == true && koncz8 == false)
                {
                    urzadzenia.AddRange(skan8.ZnalezioneAdresy); ;
                    koncz8 = true;
                }

                if (skan9.Czy_koniec == true && koncz9 == false)
                {
                    urzadzenia.AddRange(skan9.ZnalezioneAdresy); ;
                    koncz9 = true;
                }

                if (skan10.Czy_koniec == true && koncz10 == false)
                {
                    urzadzenia.AddRange(skan10.ZnalezioneAdresy); ;
                    koncz10 = true;
                }

                postep = skan1.J + skan2.J + skan3.J + skan4.J + skan5.J + skan6.J + skan7.J + skan8.J + skan9.J + skan10.J;

                if (postep > 255)
                {
                    postep = 255;
                }

                if (postep >=255)
                {
                    RunOnUiThread(() =>
                    {
                        adapter.Clear();
                        adapter.AddAll(urzadzenia);
                        textview2.Text = "Zakończono, znalezione urządzienia: " + urzadzenia.Count;
                        Skan.Enabled = true;
                        Stop.Enabled = false;
                        Toast.MakeText(this, "Zakończyłem skan!", ToastLength.Short).Show();
                    });                    
                }

                RunOnUiThread(() =>
                {
                    textview1.Text = wybranasiec_trim + postep;
                    progressbar.Progress = postep;
                });

                Thread.Sleep(500);
            } while (postep < 255 || koncz1 == false || koncz2 == false || koncz3 == false || koncz4 == false || koncz5 == false || koncz6 == false || koncz7 == false || koncz8 == false || koncz9 == false || koncz10 == false);

            
        }
    }

    public class Skan
    {
        List<string> znalezione_adresy = new List<string>();

        int zakres = 0;
        string adres_sieci = "192.168.1.";
        int j = 0, zakres_od = 0, zakres_do = 25;
        bool stop = false;
        bool czy_koniec = false;

        public Skan(string adres, int z_od, int z_do, bool stop)
        {
            this.adres_sieci = adres;
            this.zakres_od = z_od;
            this.zakres_do = z_do;
            this.stop = stop;
        }

        public void Ustaw_zakres(int z_od, int z_do)
        {
            zakres_od = z_od;
            zakres_do = z_do;
        }
        public int J
        {
            get
            {
                return j;
            }
        }
        public List<string> ZnalezioneAdresy
        {
            get
            {
                return znalezione_adresy;
            }
        }
        public bool Czy_koniec
        {
            get
            {
                return czy_koniec;
            }

            set
            {
                czy_koniec = value;
            }
        }
        public bool Stop
        {
            set
            {
                stop = value;
            }
        }

        public void skan()
        {
            znalezione_adresy.Clear();
            Ping ping;
            PingReply reply;
            IPAddress address;
            IPHostEntry host;

            for (int i = zakres_od; i <= zakres_do; i++)
            {
                if (stop == true)
                {
                    j = 0;
                    znalezione_adresy.Clear();
                    break;
                }

                ping = new Ping();
                try
                {
                    reply = ping.Send(adres_sieci + i.ToString(), 900);
                    if (reply.Status == IPStatus.Success)
                    {
                        address = IPAddress.Parse(adres_sieci + i.ToString());
                        host = Dns.GetHostEntry(address);
                        znalezione_adresy.Add(adres_sieci + i.ToString());
                    }
                }
                catch (Exception) { }
                j++;
            }
            czy_koniec = true;
        }
    }
}

