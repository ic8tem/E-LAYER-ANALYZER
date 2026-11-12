// YA-MS - finestre di messaggio (Avalonia non ha MessageBox) e apertura dei file con il programma predefinito
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace YAMS
{
    public static class Dialogs
    {
        // Messaggio con il solo pulsante OK
        public static async Task Message(Window owner, string title, string text)
        {
            StackPanel buttons;
            Window w = Build(title, text, out buttons);
            Button ok = Btn("OK", true);
            ok.Click += (s, e) => w.Close();
            buttons.Children.Add(ok);
            await ShowAsync(w, owner);
        }

        // Domanda Si'/No: true = Si'
        public static async Task<bool> Confirm(Window owner, string title, string text)
        {
            StackPanel buttons;
            Window w = Build(title, text, out buttons);
            bool result = false;
            Button yes = Btn(Lang.T("Sì"), true);
            Button no = Btn(Lang.T("No"), false);
            yes.Click += (s, e) => { result = true; w.Close(); };
            no.Click += (s, e) => { result = false; w.Close(); };
            buttons.Children.Add(yes);
            buttons.Children.Add(no);
            await ShowAsync(w, owner);
            return result;
        }

        // Scelta tra piu' pulsanti: restituisce l'indice del pulsante premuto (-1 se la finestra si chiude)
        public static async Task<int> Choice(Window owner, string title, string text, params string[] labels)
        {
            StackPanel buttons;
            Window w = Build(title, text, out buttons);
            int result = -1;
            for (int i = 0; i < labels.Length; i++)
            {
                int k = i;
                Button b = Btn(labels[i], i == 0);
                b.Click += (s, e) => { result = k; w.Close(); };
                buttons.Children.Add(b);
            }
            await ShowAsync(w, owner);
            return result;
        }

        // Apre un file con il programma predefinito del sistema (Windows, Mac, Linux)
        public static void OpenWithDefaultApp(string path)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
                return;
            }
            string cmd = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "open" : "xdg-open";
            ProcessStartInfo p = new ProcessStartInfo(cmd);
            p.ArgumentList.Add(path);
            p.UseShellExecute = false;
            Process.Start(p);
        }

        static async Task ShowAsync(Window w, Window owner)
        {
            w.Opened += (s, e) => UiScale.Apply(w);   // stessa scala del carattere della finestra principale
            if (owner != null && owner.IsVisible)
            {
                await w.ShowDialog(owner);
                return;
            }
            // senza finestra principale visibile: finestra normale, si attende la chiusura
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            w.Closed += (s, e) => tcs.TrySetResult(true);
            w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            w.Show();
            await tcs.Task;
        }

        static Button Btn(string text, bool accent)
        {
            Button b = new Button { Content = text, MinWidth = 90, HorizontalContentAlignment = HorizontalAlignment.Center };
            b.Classes.Add("pill");
            if (accent) b.Classes.Add("accent");
            return b;
        }

        static Window Build(string title, string text, out StackPanel buttons)
        {
            Window w = new Window
            {
                Title = title,
                Width = 560 * Math.Min(2.0, UiScale.Font),   // piu' larga con il carattere ingrandito
                SizeToContent = SizeToContent.Height,
                CanResize = false,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Background = Pal.B(Pal.Panel),
                Foreground = Pal.B(Pal.Fore)
            };
            TextBlock head = new TextBlock
            {
                Text = title,
                FontWeight = FontWeight.Bold,
                FontSize = 15,
                Foreground = Pal.B(Pal.Azure),
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 10)
            };
            SelectableTextBlock body = new SelectableTextBlock
            {
                Text = (text ?? "").Replace("\r\n", "\n"),
                TextWrapping = TextWrapping.Wrap,
                FontSize = 14,
                Foreground = Pal.B(Pal.Fore)
            };
            ScrollViewer sv = new ScrollViewer { MaxHeight = 520, Content = body };
            buttons = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 8,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 16, 0, 0)
            };
            StackPanel sp = new StackPanel();
            sp.Children.Add(head);
            sp.Children.Add(sv);
            sp.Children.Add(buttons);
            w.Content = new Border
            {
                BorderBrush = Pal.B(Pal.Border),
                BorderThickness = new Thickness(1),
                Padding = new Thickness(20, 16),
                Child = sp
            };
            return w;
        }
    }
}
