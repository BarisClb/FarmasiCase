# FarmasiCase

### Proje Hakkında

- Proje içerisinde 3 adet Entity mevcut : Product, Order ve User. 
- Bunlara ek olarak Account ve Cart Serviceleri ve Controller'ları mevcut.
   - Product, Order ve User Crud işlemlere sahip. 
   - Cart servisi, Database ve Redis Cache arasında işlem yapıyor.
   - OrderService ise cachedeki bu Cart listi alıp, User bilgisi ile birlikte Database'de bir Order yaratıyor. 
- Orderların bir sahibi (OrderedBy) olmasını istediğim için Account Service ekledim. 
- Account Servisi, User entity aracılığı ile bir hesap sistemi oluşturuyor.
   - Login, Logout ve Verify aksiyonları var. Login durumunda Jwt olarak bir Cookie ekliyor tarayıcıya. Order verileceği zaman bu cookie yi kontrol edip, eğer User login yapmışsa, User'ın da adını ekleyerek Order'ı oluşturuyor. Cookie yoksa hata veriyor.
- Database bağlantısı MongoDB Atlas aracılığı ile gerçekleşiyor.
- Redis Caching Docker aracılığı ile Local olarak yapılıyor. 
   - ConnectionString appsettings.json içerisinde mevcut (default localhost:5002).
- RabbitMQ kapsamında, başarılı işlemlerin sonuna bir mesaj döngüsü ekledim. Her bir Service için ayrı bir Queue'ya tabi tuttum.
   - ConnectionString, QueueFactory'nin tanımlandığı 'src/FarmasiCase.Service/RabbitMQ/QueueFactory' içerisinde mevcut (default localhost).