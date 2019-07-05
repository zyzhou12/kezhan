using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Util;
using System.Security.Cryptography;
using System.IO;
using System.Security.Cryptography.Xml;
using System.Runtime.InteropServices;
using System.Text;


namespace TiKu.Api.Code
{
    public class WebRTCSig
    {

        // 下面的公私钥路径都是绝对路径，请开发者自行修改
        const string pri_key_path = @"C:\KeZhan\Web-Test\keys\private_key.pem";
        const string pub_key_path = @"C:\KeZhan\Web-Test\keys\public_key.pem";

        public static string GetSig(string strUser)
        {
            // 生成 sig 文件
            FileStream f = new FileStream(pri_key_path, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(f);
            byte[] b = new byte[f.Length];
            reader.Read(b, 0, b.Length);
            string pri_key = Encoding.Default.GetString(b);

            StringBuilder sig = new StringBuilder(4096);
            StringBuilder err_msg = new StringBuilder(4096);
            int ret = sigcheck.tls_gen_sig_ex(
                1400178589,
                strUser,
                sig,
                4096,
                pri_key,
                (UInt32)pri_key.Length,
                err_msg,
                4096);
            if (0 != ret)
            {
                Console.WriteLine("err_msg: " + err_msg);
                return "";
            }
            Console.WriteLine("\n-----\n");
            Console.WriteLine("gen sig:\n\n" + sig);

            // 校验 sig
            f = new FileStream(pub_key_path, FileMode.Open, FileAccess.Read);
            reader = new BinaryReader(f);
            b = new byte[f.Length];
            reader.Read(b, 0, b.Length);
            string pub_key = Encoding.Default.GetString(b);

            UInt32 expire_time = 0;
            UInt32 init_time = 0;
            ret = sigcheck.tls_vri_sig_ex(
                sig.ToString(),
                pub_key,
                (UInt32)pub_key.Length,
                1400178589,
                strUser,
                ref expire_time,
                ref init_time,
                err_msg,
                4096);

            if (0 != ret)
            {
                Console.WriteLine("err_msg: " + err_msg);
                return "";
            }

            Console.WriteLine("\n-----\n");
            Console.WriteLine("verify ok -- expire time " + expire_time + " -- init time " + init_time);
            Console.WriteLine("\n-----\n");
            // Console.ReadKey();
            return sig.ToString();
        }
    }

    class dllpath
    {
        // 开发者调用 dll 时请注意项目的平台属性，下面的路径是 demo 创建时使用的，请自己使用予以修改
        // 请使用适当的平台 dll
        //public const string DllPath = @"D:\src\oicq64\tinyid\tls_sig_api\windows\64\lib\libsigcheck\sigcheck.dll";       // 64 位
        // 如果选择 Any CPU 平台，默认加载 32 位 dll
        public const string DllPath = @"C:\KeZhan\Web-Test\keys\sigcheck.dll";     // 32 位
    }

    class sigcheck
    {
        [DllImport(dllpath.DllPath, EntryPoint = "tls_gen_sig", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static int tls_gen_sig(
            UInt32 expire,
            string appid3rd,
            UInt32 sdkappid,
            string identifier,
            UInt32 acctype,
            StringBuilder sig,
            UInt32 sig_buff_len,
            string pri_key,
            UInt32 pri_key_len,
            StringBuilder err_msg,
            UInt32 err_msg_buff_len
        );

        [DllImport(dllpath.DllPath, EntryPoint = "tls_vri_sig", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static int tls_vri_sig(
            string sig,
            string pub_key,
            UInt32 pub_key_len,
            UInt32 acctype,
            string appid3rd,
            UInt32 sdkappid,
            string identifier,
            StringBuilder err_msg,
            UInt32 err_msg_buff_len
        );

        [DllImport(dllpath.DllPath, EntryPoint = "tls_gen_sig_ex", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static int tls_gen_sig_ex(
            UInt32 sdkappid,
            string identifier,
            StringBuilder sig,
            UInt32 sig_buff_len,
            string pri_key,
            UInt32 pri_key_len,
            StringBuilder err_msg,
            UInt32 err_msg_buff_len
        );

        [DllImport(dllpath.DllPath, EntryPoint = "tls_vri_sig_ex", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static int tls_vri_sig_ex(
            string sig,
            string pub_key,
            UInt32 pub_key_len,
            UInt32 sdkappid,
            string identifier,
            ref UInt32 expire_time,
            ref UInt32 init_time,
            StringBuilder err_msg,
            UInt32 err_msg_buff_len
        );
    }
}