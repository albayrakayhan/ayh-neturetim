-> API Help Dosyasý için descriptionlarý aktif etmek:
	https://www.asp.net/web-api/overview/getting-started-with-aspnet-web-api/creating-api-help-pages

	https://www.asp.net/web-api/overview/advanced/sending-html-form-data-part-1
	https://www.asp.net/web-api/overview/advanced/sending-html-form-data-part-2
	http://www.dotnetcurry.com/aspnet/1278/aspnet-webapi-pass-multiple-parameters-action-method

	https://msdn.microsoft.com/en-us/library/ms131092.aspx

	http://92.45.23.86:9597/api/cari/finans-bilgileririni-getir?carikart_id=100000000001 çalýþýyor
	http://92.45.23.86:9597/api/cari/finans-bilgileririni-getir?carikart_id=100000000963 çalýþmýyor




*********stokkart Ekler için
Select * from stokkart_ekler

Select * from ekler


Select 
ekturu_id,
tanim,
file_ext,
file_types
from giz_setup_ekturu
WHERE stokkart_tipi_id = 1 AND statu = 1

*********

--kullanýcý yetki gruplarý
SELECT * FROM [dbo].[giz_setup_kullanici_yetki_grup]

--kullanýcýnýn baðlý olduðu gruplar
select * from kullanicigruplari

--yetki alanlarý
select * from [dbo].[giz_sabit_yetki_alanlari]

--kullanýcý bazlý yetkiler
select * from [dbo].[kullaniciyetki]

--grup bazlý yetkiler
select * from [dbo].[kullanicigrupyetki]

--bir kullanýcýnýn yetkileri
select * from [dbo].[GetKullaniciYetki]('URETIM', 100000000100)
Where url in ('api/stokkart','api/stokkart/arama','api/stokkart/stok-kod-arama')

--------------------------------------

Select * from giz_sabit_stokkarttipi
Select * from giz_sabit_stokkartturu