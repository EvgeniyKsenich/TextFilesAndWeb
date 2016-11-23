using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TextFilesAndWeb
{
    class Program
    {
        static void Main(string[] args)
        {
            try{
                Regex t1 = new Regex(@"(?:(((http|https):\/\/)([^\W][a-zA-Z\d\.-]+\.)([a-z]+))\-)");
                MatchCollection tmp = t1.Matches(" ");
                Console.WriteLine("Enter start url: ");
                string url = Console.ReadLine();
                string path_URL = @"F:\html_pr\3\urls.txt";
                string path_MAIL = @"F:\html_pr\3\mail.txt";
                bool tr = true;
                while (tr)
                {
                    string DATA = newData(url);
                    MatchCollection newAllUrl = getAllUrl(DATA);
                    MatchCollection Newmail = getAllemail(DATA);
                    write(path_URL, newAllUrl, true);
                    write(path_MAIL, Newmail, false);
                    while (true) {
                    url = NewRNDurl(newAllUrl);
                    if (cheakOne(path_URL,url)) break;
                    }
                    appearnd(path_URL, url);
                    tmp = t1.Matches(" ");
                }
            }
            catch(Exception e)
            {

            }
        }

        static bool cheackAll(string path, MatchCollection url)
        {
            string all = "";
            using (StreamReader sr = new StreamReader(path, Encoding.GetEncoding(1251)))
            {
                all = sr.ReadToEnd();
            }
            int lic = 0;
            if (url.Count == 0) return false;
            for (int i = 0; i < url.Count; i++)
            {
                if (!File.Exists(path)) return true;
                else
                {
                    if (all.Contains(url[i].Groups[0].Value))
                    {
                        int a = all.IndexOf(url[i].Groups[0].Value, StringComparison.CurrentCulture);
                        a += url[i].Groups[0].Length;
                        a++;
                        if (all[a] == '+') lic++;
                    }
                }
            }
            if (lic == url.Count) return false;
            else
                return false;
        }

        static bool cheakOne(string path, string url)
        {
            if (!File.Exists(path)) return true;
            else
            {
                string all = "";
                using (StreamReader sr = new StreamReader(path, Encoding.GetEncoding(1251)))
                {
                    all = sr.ReadToEnd();
                }
                if (all.Contains(url))
                {
                    int a = all.IndexOf(url, StringComparison.CurrentCulture);
                    a += url.Length;
                    a++;
                    if (all[a] == '+') return false;
                    else return true;
                }
                else return true;
            }
        }

        static string NewRNDurl(MatchCollection tmp)
        {
            int cout = tmp.Count;
            Random rnd = new Random();
            int r = rnd.Next(1, cout+1);
            return tmp[r-1].Groups[0].Value;
        }

        static void appearnd(string path, string url)
        {
            string all = "";
            using (StreamReader sr = new StreamReader(path, Encoding.GetEncoding(1251)))
            {
                all = sr.ReadToEnd();
            }
            int a = all.IndexOf(url, StringComparison.CurrentCulture);
            string[] mas = GetMas(all);
            for (int i = 0; i < mas.Length - 1; i++)
            {
                //Console.WriteLine(mas[i]);
                if (mas[i].Contains(url))
                {
                    a += url.Length;
                    mas[i] = mas[i].Replace("-", "+");
                    Console.WriteLine(mas[i]);
                }
            }

            write(path, mas);
        }
        static void write(string path, string[] file)
        {
            string file1 = "";
            for (int i = 0; i < file.Length; i++)
            {
                file1 += file[i] + Environment.NewLine;
            }
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(file1);
            }
        }

        static string[] GetMas(string all)
        {
            int cout = 0, lenth = all.Length;
            for (int i = 0; i < lenth; i++)
            {
                if (all[i].ToString() == "\n") cout++;
            }
            cout++;
            //Console.WriteLine(cout.ToString());
            string[] mas = new string[cout];
            for (int i = 0, i1 = 0; i < lenth; i++)
            {
                if (all[i].ToString() != "\n") mas[i1] += all[i];
                if (all[i].ToString() == "\n") i1++;
            }
            return mas;
        }

        static MatchCollection getAllUrl(string DATA)
        {
            Regex t1 = new Regex(@"\b(((http|https):\/\/)(www\.)?([^\W][a-zA-Z\d\.-]+\.)([a-z]+)(\/[a-z]+)?)\b");
            MatchCollection tmp = t1.Matches(DATA);
            return tmp;
        }

        static MatchCollection getAllemail(string DATA)
        {
            Regex t1 = new Regex(@"((?:(?:[0-9a-zA-Z.\-_]){1,}[^.@\-_])@(?:(?:[\d]|[a-zA-Z]){1,})(?:\.(?:[\d]|[a-zA-Z]){1,})+)");
            MatchCollection tmp = t1.Matches(DATA);
            return tmp;
        }

        static bool isUniq(string path, string objecT)
        {
            if (!File.Exists(path)) return true;
            else
            {
                string all = "";
                using (StreamReader sr = new StreamReader(path, Encoding.GetEncoding(1251)))
                {
                    all = sr.ReadToEnd();
                }
                if (all.Contains(objecT)) return false;
                else return true;
            }
        }

        static void write(string path, string file)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(file);
            }
        }

        static void write(string path, MatchCollection file, bool code)
        {
            string fl = "";
            int coute = file.Count;
            bool cheak = false;
            for (int i = 0; i < coute; i++)
            {
                cheak = isUniq(path, file[i].Groups[0].Value);
                if (File.Exists(path))
                {
                    using (StreamWriter sw = new StreamWriter(path, true))
                    {
                        if (cheak)
                        {
                            fl = file[i].Groups[0].Value;
                            if (code) fl += "-" + Environment.NewLine;
                            else fl += Environment.NewLine;
                            sw.Write(fl);
                        }
                        sw.Close();
                    }
                }
                else
                {
                    using (StreamWriter sw = new StreamWriter(path))
                    {
                        fl = file[i].Groups[0].Value;
                        if (code) fl += "-" + Environment.NewLine;
                        else fl += Environment.NewLine;
                        sw.Write(fl);
                    }

                }
            }
        }

        static string newData(string url)
        {
            WebRequest request = WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            return reader.ReadToEnd();
        }
    }
}

