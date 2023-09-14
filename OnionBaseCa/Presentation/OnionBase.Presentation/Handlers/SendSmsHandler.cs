//using OnionBase.Presentation.Commands;
//using OnionBase.Presentation.Interfaces;
//using OnionBase.Persistance.Contexts;

//namespace OnionBase.Presentation.Handlers
//{
//    public class SendSmsHandler
//    {
//        private readonly UserDbContext _context;
//        private readonly ISmsHelper _smsHelper;
//        public SendSmsHandler(UserDbContext context, ISmsHelper smsHelper)
//        {
//            _context = context;
//            _smsHelper = smsHelper;
//        }
//        public async Task<string> Handle(SmsVerify smsVerify, CancellationToken cancellationToken)
//        {
//            var person = await _context.UserDatas.FindAsync(smsVerify.User.Id);
//            if (person == null)
//            {
//                throw new ArgumentException("Kullanıcı bulunamadı !");
//            }
//            if ((bool)(person.SmsVerify == false))
//            {
//                throw new ArgumentException("Sms Doğrulaması Yapılmamış.");
//            }
//            Random rand = new Random();
//            int number = rand.Next(100000, 1000000);
//            person.sentCode = number;
//            await _context.SaveChangesAsync();
//            string message = "Merhaba, Sms kodunuz: {0}";
//            await _smsHelper.SendSms(string.Format(message, number), person.PhoneNumber);
//            return person.Id;
//        }
//    }
//}
