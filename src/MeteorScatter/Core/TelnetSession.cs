// YA-MS (dal programma E-LAYER ANALYZER) - sessione telnet verso un DX cluster (login automatico, comandi iniziali, riconnessione)
// Le righe ricevute arrivano dall'evento LineReceived su un thread secondario: l'interfaccia le passa al proprio thread.
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YAMS
{
    public class TelnetSession
    {
        public string Name = "";
        public string Host = "";
        public int Port = 23;
        public string Login = "";
        public string InitCommands = "";     // comandi dopo il login, separati da ';'
        public bool AutoReconnect = true;

        public event Action<string> LineReceived;      // riga di testo ricevuta
        public event Action<string> StateChanged;      // descrizione dello stato

        public bool Connected { get { return client != null && client.Connected && running; } }
        public int LinesIn, SpotsIn;

        TcpClient client;
        NetworkStream stream;
        CancellationTokenSource cts;
        volatile bool running, wantConnected, loggedIn;
        readonly object wlock = new object();

        public void Connect()
        {
            wantConnected = true;
            if (running) return;
            Task.Run(() => Loop());
        }

        public void Disconnect()
        {
            wantConnected = false;
            try { if (cts != null) cts.Cancel(); } catch { }
            try { if (client != null) client.Close(); } catch { }
        }

        public void Send(string text)
        {
            try
            {
                NetworkStream s = stream;
                if (s == null) return;
                byte[] b = Encoding.ASCII.GetBytes(text + "\r\n");
                lock (wlock) s.Write(b, 0, b.Length);
                Raise("> " + text);
            }
            catch (Exception ex) { State("Errore di invio: " + ex.Message); }
        }

        void Raise(string line) { Action<string> a = LineReceived; if (a != null) a(line); }
        void State(string s) { Action<string> a = StateChanged; if (a != null) a(s); }

        async Task Loop()
        {
            running = true;
            while (wantConnected)
            {
                cts = new CancellationTokenSource();
                loggedIn = false;
                try
                {
                    if (string.IsNullOrWhiteSpace(Host)) { State("Indirizzo mancante"); break; }
                    State("Connessione a " + Host + ":" + Port + "...");
                    client = new TcpClient();
                    Task ct = client.ConnectAsync(Host.Trim(), Port);
                    if (await Task.WhenAny(ct, Task.Delay(15000)) != ct) throw new IOException("tempo scaduto");
                    await ct;
                    stream = client.GetStream();
                    State("Connesso a " + Host + ":" + Port);
                    await ReadLoop(cts.Token);
                    State("Disconnesso");
                }
                catch (Exception ex)
                {
                    State("Errore: " + ex.Message);
                }
                finally
                {
                    try { if (client != null) client.Close(); } catch { }
                    stream = null;
                }
                if (!wantConnected || !AutoReconnect) break;
                State("Nuovo tentativo tra 30 s");
                for (int i = 0; i < 30 && wantConnected; i++) await Task.Delay(1000);
            }
            running = false;
            State(wantConnected ? "Disconnesso" : "Chiuso");
        }

        async Task ReadLoop(CancellationToken tok)
        {
            byte[] buf = new byte[4096];
            StringBuilder cur = new StringBuilder();
            while (!tok.IsCancellationRequested)
            {
                int n = await stream.ReadAsync(buf, 0, buf.Length, tok);
                if (n <= 0) break;
                List<byte> reply = null;
                for (int i = 0; i < n; i++)
                {
                    byte b = buf[i];
                    if (b == 255 && i + 1 < n)   // IAC: negoziazione telnet, si rifiuta tutto
                    {
                        byte cmd = buf[i + 1];
                        if (cmd >= 251 && cmd <= 254 && i + 2 < n)
                        {
                            byte opt = buf[i + 2];
                            if (cmd == 253 || cmd == 251)   // DO -> WONT, WILL -> DONT
                            {
                                if (reply == null) reply = new List<byte>();
                                reply.Add(255); reply.Add(cmd == 253 ? (byte)252 : (byte)254); reply.Add(opt);
                            }
                            i += 2;
                        }
                        else i += 1;
                        continue;
                    }
                    char c = (char)b;
                    if (c == '\n')
                    {
                        string line = cur.ToString().TrimEnd('\r');
                        cur.Clear();
                        LinesIn++;
                        Raise(line);
                    }
                    else if (c != '\0' && c != '\a') cur.Append(c);
                }
                if (reply != null && reply.Count > 0)
                {
                    byte[] rb = reply.ToArray();
                    lock (wlock) stream.Write(rb, 0, rb.Length);
                }
                // richiesta del nominativo (il prompt non ha l'a capo)
                if (!loggedIn)
                {
                    string p = cur.ToString().ToLowerInvariant();
                    if (p.Contains("login") || p.Contains("call") || p.Contains("callsign") || p.Contains("name:"))
                    {
                        Raise(cur.ToString());
                        cur.Clear();
                        DoLogin();
                    }
                }
            }
        }

        void DoLogin()
        {
            if (loggedIn) return;
            loggedIn = true;
            if (string.IsNullOrWhiteSpace(Login)) { State("Manca il nominativo per il login"); return; }
            Send(Login.Trim());
            string cmds = InitCommands ?? "";
            Task.Run(async () =>
            {
                await Task.Delay(1500);
                foreach (string c in cmds.Split(';'))
                {
                    if (c.Trim().Length == 0) continue;
                    Send(c.Trim());
                    await Task.Delay(800);
                }
            });
        }
    }
}
