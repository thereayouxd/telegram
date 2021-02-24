using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TeleSharp.TL;
using TLSharp.Core;

namespace TLMessage
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //Id API 
        static  int apiId = 3691496;
        //Создани клиент для подключения к телеграм
        TelegramClient client = new TelegramClient(apiId, apiHash);
        //APIhash
        static  string apiHash = "b08ca429fc73f46d1846622e26a75e0a";

        private async void button1_Click(object sender, EventArgs e)
        {
             if (client.IsUserAuthorized())
             {
                 //сообщение
                 string message = textBox1.Text;
                 //получатель 
                 string recipient = textBox2.Text;
                 //подключаемся
                 await client.ConnectAsync();
                 //выбираем список контактов отправителя
                 var result = await client.GetContactsAsync();
                 //создаем список контактов
                 TLUser user = new TLUser();

                 if (string.IsNullOrWhiteSpace(recipient))
                 {

                     MessageBox.Show("Введите телефон получателя!", "Ошибка!");

                 }
                 else
                 {
                     //выбираем пользователя по номеру телефона из списка контактов
                     user = result.Users
                         .Where(x => x.GetType() == typeof(TLUser))
                         .Cast<TLUser>()
                         .FirstOrDefault(x => x.Phone == textBox2.Text);
                 }
                 if (string.IsNullOrWhiteSpace(message))
                 {
                     MessageBox.Show("введите сообщение!", "Ошибка!");
                 }
                 else
                 {
                     try
                     {
                         //отправляем сообщение
                         await client.SendMessageAsync(new TLInputPeerUser() { UserId = user.Id }, message);
                         textBox1.Text = "";
                     }
                     catch (Exception ex)
                     {
                         MessageBox.Show("Данного номера нет в вашем списке контактов!");

                     }
                 }
             }
             else
             {
                //создаем сессию
                var store = new FileSessionStore();

                //подключаемся к телеграм
                await client.ConnectAsync();

                //отправляем код потверждения на наш номер
                var hash = await client.SendCodeRequestAsync("79807545992");

                //следует поменять на код присланный телеграмом в дебаге
                var code = "46222";
                //потверждаем нашу учетную запись
                var user = await client.MakeAuthAsync("79807545992", hash, code);
            }
          
           
        }

        //метод для аутентификации 
        public async Task AuthUser()
        {

            //создаем сессию
            var store = new FileSessionStore();

            //подключаемся к телеграм
            await client.ConnectAsync();

            //отправляем код потверждения на наш номер
            var hash = await client.SendCodeRequestAsync("79807545992");

            //следует поменять на код присланный телеграмом в дебаге
            var code = "80173";
            //потверждаем нашу учетную запись
            var user = await client.MakeAuthAsync("79807545992", hash, code);

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            //ввод в текст бокс только цифр
            if (!Char.IsDigit(number) && number!=8)
            {
                e.Handled = true;
            }
        }
    }
}
