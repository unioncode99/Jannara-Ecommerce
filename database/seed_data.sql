USE [Jannara];
GO

-- =============================================
-- 1. Brands
-- =============================================
INSERT INTO Brands (name_en, name_ar, logo_url, website_url, description_en, description_ar, created_at, updated_at)
VALUES
('Samsung', N'سامسونج', 'https://upload.wikimedia.org/wikipedia/commons/2/24/Samsung_Logo.svg', 'https://www.samsung.com', 'Samsung is a global leader in electronics.', N'سامسونج شركة عالمية للإلكترونيات.', GETDATE(), GETDATE()),
('Apple', N'آبل', 'https://upload.wikimedia.org/wikipedia/commons/f/fa/Apple_logo_black.svg', 'https://www.apple.com', 'Apple makes iconic devices.', N'آبل تصنع أجهزة أيقونية.', GETDATE(), GETDATE()),
('Sony', N'سوني', 'https://upload.wikimedia.org/wikipedia/commons/2/29/Sony_logo.svg', 'https://www.sony.com', 'Sony leads in electronics and entertainment.', N'سوني رائدة في الإلكترونيات والترفيه.', GETDATE(), GETDATE()),
('LG', N'إل جي', 'https://upload.wikimedia.org/wikipedia/commons/2/20/LG_symbol.svg', 'https://www.lg.com', 'LG smart home and entertainment solutions.', N'إل جي حلول ذكية للمنزل والترفيه.', GETDATE(), GETDATE()),
('Nike', N'نايكي', 'https://upload.wikimedia.org/wikipedia/commons/a/a6/Logo_NIKE.svg', 'https://www.nike.com', 'Nike athletic wear and shoes.', N'نايكي للملابس والأحذية الرياضية.', GETDATE(), GETDATE()),
('Adidas', N'أديداس', 'https://upload.wikimedia.org/wikipedia/commons/2/20/Adidas_Logo.svg', 'https://www.adidas.com', 'Adidas sport and lifestyle brand.', N'أديداس علامة رياضية وعصرية.', GETDATE(), GETDATE());
GO

-- =============================================
-- 2. ProductCategories
-- =============================================
INSERT INTO ProductCategories (parent_category_id, name_en, name_ar, description_en, description_ar)
VALUES
(NULL, 'Electronics', N'الإلكترونيات', 'Electronic devices and gadgets', N'الأجهزة الإلكترونية والأدوات الذكية'),
(NULL, 'Mobile Phones', N'الهواتف الذكية', 'Smartphones & accessories', N'الهواتف الذكية وملحقاتها'),
(NULL, 'Computers & Laptops', N'أجهزة الكمبيوتر واللابتوب', 'Laptops, desktops & accessories', N'أجهزة الكمبيوتر المحمولة والمكتبية وملحقاتها'),
(NULL, 'Home Appliances', N'الأجهزة المنزلية', 'Appliances for home and kitchen', N'أجهزة للمنزل والمطبخ'),
(NULL, 'Fashion', N'الأزياء', 'Clothing and fashion items', N'ملابس وعناصر الموضة'),
(NULL, 'Men Clothing', N'ملابس رجالية', 'Fashion for men', N'أزياء للرجال'),
(NULL, 'Women Clothing', N'ملابس نسائية', 'Fashion for women', N'أزياء للنساء'),
(NULL, 'Accessories', N'الإكسسوارات', 'Tech and fashion accessories', N'إكسسوارات تقنية وعصرية');
GO

-- =============================================
-- 3. Products (96 total: 12 per category)
-- =============================================
-- Note: For readability, category blocks are grouped

-- Electronics (CategoryId = 1)
INSERT INTO Products (category_id, brand_id, default_image_url, name_en, name_ar, description_en, description_ar)
VALUES
(1,2,'https://picsum.photos/400?random=201','Apple iPhone 15',N'آيفون 15','Latest Apple iPhone with A17 chipset.',N'هاتف آيفون 15 بأحدث معالج A17.'),
(1,1,'https://picsum.photos/400?random=202','Samsung Galaxy S24',N'سامسونج جالاكسي S24','Flagship Samsung smartphone.',N'هاتف سامسونج S24 الرائد.'),
(1,3,'https://picsum.photos/400?random=203','Sony WH-1000XM5',N'سوني WH-1000XM5','Premium noise-cancelling headphones.',N'سماعات بخاصية إلغاء الضوضاء.'),
(1,2,'https://picsum.photos/400?random=204','Apple MacBook Pro 16',N'ماك بوك برو 16','Powerful laptop with M3 chip.',N'لابتوب قوي بمعالج M3.'),
(1,1,'https://picsum.photos/400?random=205','Samsung Galaxy Tab S9',N'سامسونج تاب S9','High-performance Android tablet.',N'جهاز لوحي أندرويد عالي الأداء.'),
(1,2,'https://picsum.photos/400?random=206','Apple iPad Pro 12.9',N'آيباد برو 12.9','iPad with Liquid Retina display.',N'آيباد بشاشة ليكويد ريتينا.'),
(1,3,'https://picsum.photos/400?random=207','Sony Alpha A7 IV',N'سوني ألفا A7 IV','Full-frame mirrorless camera.',N'كاميرا بدون مرآة وإطار كامل.'),
(1,1,'https://picsum.photos/400?random=208','LG OLED TV 55',N'إل جي OLED 55','4K OLED smart TV.',N'تلفاز ذكي بدقة 4K وOLED.'),
(1,2,'https://picsum.photos/400?random=209','Apple AirPods Pro 2',N'آيربودز برو 2','Wireless earbuds with ANC.',N'سماعات لاسلكية مع إلغاء ضوضاء.'),
(1,3,'https://picsum.photos/400?random=210','Sony PlayStation 5',N'بلاي ستيشن 5','Next-gen gaming console.',N'جهاز ألعاب الجيل التالي.'),
(1,1,'https://picsum.photos/400?random=211','LG Gram 16 Laptop',N'إل جي جرام 16','Ultra-light laptop with long battery.',N'لابتوب خفيف مع بطارية طويلة.'),
(1,2,'https://picsum.photos/400?random=212','Samsung SSD 1TB',N'سامسونج SSD 1 تيرابايت','High-speed internal SSD.',N'قرص تخزين سريع داخلي.');

-- Mobile Phones (CategoryId = 2)
INSERT INTO Products (category_id, brand_id, default_image_url, name_en, name_ar, description_en, description_ar)
VALUES
(2,2,'https://picsum.photos/400?random=221','iPhone 15 Pro',N'آيفون 15 برو','Apple iPhone 15 Pro with triple camera.',N'آيفون 15 برو بكاميرا ثلاثية.'),
(2,1,'https://picsum.photos/400?random=222','Samsung Galaxy A54',N'سامسونج جالاكسي A54','Mid-range Galaxy phone.',N'هاتف جالاكسي A54 متوسط الفئة.'),
(2,2,'https://picsum.photos/400?random=223','Apple iPhone SE',N'آيفون SE','Compact Apple smartphone.',N'هاتف آيفون SE صغير الحجم.'),
(2,1,'https://picsum.photos/400?random=224','Samsung Galaxy Z Flip',N'سامسونج Z فليب','Foldable smartphone.',N'هاتف قابل للطي.'),
(2,3,'https://picsum.photos/400?random=225','Sony Xperia 10',N'سوني اكسبيريا 10','Sony mid-range phone.',N'هاتف سوني متوسط الفئة.'),
(2,2,'https://picsum.photos/400?random=226','Apple iPhone 14',N'آيفون 14','Previous gen iPhone.',N'آيفون الجيل السابق.'),
(2,1,'https://picsum.photos/400?random=227','Samsung Galaxy Note 20',N'سامسونج نوت 20','Note series smartphone.',N'هاتف سلسلة نوت.'),
(2,2,'https://picsum.photos/400?random=228','Apple iPhone 13 Mini',N'آيفون 13 ميني','Compact Apple phone.',N'هاتف آبل ميني.'),
(2,1,'https://picsum.photos/400?random=229','Samsung Galaxy M33',N'سامسونج M33','Large battery phone.',N'هاتف ببطارية كبيرة.'),
(2,3,'https://picsum.photos/400?random=230','Sony Xperia 1 IV',N'سوني اكسبيريا 1 IV','Flagship Sony phone.',N'هاتف سوني الرائد.'),
(2,2,'https://picsum.photos/400?random=231','Apple iPhone XR',N'آيفون XR','Classic Apple phone.',N'آيفون قديم.'),
(2,1,'https://picsum.photos/400?random=232','Samsung Galaxy S22',N'سامسونج S22','Galaxy S22 smartphone.',N'هاتف جالاكسي S22.');

-- Computers & Laptops (CategoryId = 3)
INSERT INTO Products (category_id, brand_id, default_image_url, name_en, name_ar, description_en, description_ar)
VALUES
(3,2,'https://picsum.photos/400?random=241','MacBook Air M2',N'ماك بوك Air M2','Thin laptop with M2 chip.',N'لابتوب نحيف بمعالج M2.'),
(3,1,'https://picsum.photos/400?random=242','Samsung Notebook 9',N'سامسونج نوتبوك 9','Premium Samsung laptop.',N'لابتوب سامسونج فاخر.'),
(3,3,'https://picsum.photos/400?random=243','Sony VAIO Z',N'سوني VAIO Z','Sony laptop series.',N'سلسلة لابتوب سوني.'),
(3,2,'https://picsum.photos/400?random=244','Apple Mac Mini',N'ماك ميني','Compact desktop computer.',N'كمبيوتر ميني مدمج.'),
(3,1,'https://picsum.photos/400?random=245','Samsung Desktop All‑in‑One',N'سامسونج كل‑في‑واحد','All‑in‑one PC.',N'كمبيوتر مكتبي الكل في واحد.'),
(3,2,'https://picsum.photos/400?random=246','Apple iMac 24',N'آي ماك 24','Apple desktop.',N'كمبيوتر مكتبي آبل.'),
(3,3,'https://picsum.photos/400?random=247','Sony UltraPortable Laptop',N'سوني لابتوب محمول','Portable laptop.',N'لابتوب محمول سهل الحمل.'),
(3,2,'https://picsum.photos/400?random=248','Apple Studio Display',N'ستوديو ديسبلاي','High‑end monitor.',N'شاشة عالية الجودة.'),
(3,1,'https://picsum.photos/400?random=249','Samsung SSD 2TB',N'سامسونج SSD 2 تيرابايت','Large capacity SSD.',N'قرص SSD بسعة كبيرة.'),
(3,3,'https://picsum.photos/400?random=250','Sony Professional Workstation',N'سوني وورك ستيشن','High‑performance workstation.',N'وورك ستيشن عالي الأداء.'),
(3,2,'https://picsum.photos/400?random=251','Apple Mac Studio',N'ماك ستوديو','Performance desktop.',N'كمبيوتر أداء عالي.'),
(3,1,'https://picsum.photos/400?random=252','Samsung Portable Monitor',N'سامسونج شاشة محمولة','Portable screen.',N'شاشة محمولة.');

-- Home Appliances (CategoryId = 4)
INSERT INTO Products (category_id, brand_id, default_image_url, name_en, name_ar, description_en, description_ar)
VALUES
(4,1,'https://picsum.photos/400?random=261','Samsung Refrigerator',N'ثلاجة سامسونج','Energy‑efficient fridge.',N'ثلاجة موفرة للطاقة.'),
(4,1,'https://picsum.photos/400?random=262','Samsung Washing Machine',N'غسالة سامسونج','Top‑load washing machine.',N'غسالة أعلى تحميل.'),
(4,4,'https://picsum.photos/400?random=263','LG Smart Dryer',N'مجفف ذكي إل جي','Smart drying solutions.',N'حلول تجفيف ذكية.'),
(4,4,'https://picsum.photos/400?random=264','LG Air Conditioner',N'مكيف هواء إل جي','Split AC with inverter.',N'مكيف انفرتر فعال.'),
(4,2,'https://picsum.photos/400?random=265','Apple HomePod',N'هوم بود آبل','Smart home speaker.',N'مكبر صوت ذكي للمنزل.'),
(4,3,'https://picsum.photos/400?random=266','Sony Vacuum Cleaner',N'مكنسة سوني','Powerful vacuum.',N'مكنسة قوية.'),
(4,1,'https://picsum.photos/400?random=267','Samsung Microwave Oven',N'فرن ميكروويف سامسونج','Compact microwave.',N'ميكروويف صغير.'),
(4,4,'https://picsum.photos/400?random=268','LG Dishwasher',N'غسالة صحون إل جي','Efficient dishwasher.',N'غسالة صحون فعالة.'),
(4,2,'https://picsum.photos/400?random=269','Apple Smart Fridge',N'ثلاجة ذكية آبل','Fridge with smart features.',N'ثلاجة بميزات ذكية.'),
(4,3,'https://picsum.photos/400?random=270','Sony Smart Heater',N'سوني سخان ذكي','Smart heater with remote.',N'سخان ذكي مع ريموت.'),
(4,1,'https://picsum.photos/400?random=271','Samsung Robot Vacuum',N'مكنسة روبوت','Robot vacuum cleaner.',N'مكنسة روبوت.'),
(4,4,'https://picsum.photos/400?random=272','LG Smart Washer dryer',N'ال غسالة مجفف ذكي','Washer & dryer combo.',N'غسالة ومجفف في جهاز واحد.');

-- Fashion (CategoryId = 5)
INSERT INTO Products (category_id, brand_id, default_image_url, name_en, name_ar, description_en, description_ar)
VALUES
(5,5,'https://picsum.photos/400?random=281','Nike Air Max 90',N'نايكي إير ماكس 90','Iconic Nike sneakers.',N'أحذية نايكي أيقونية.'),
(5,6,'https://picsum.photos/400?random=282','Adidas Ultraboost',N'أديداس ألترا بوست','Performance running shoes.',N'أحذية جري عالية الأداء.'),
(5,5,'https://picsum.photos/400?random=283','Nike Dri‑Fit T‑Shirt',N'نايكي قميص','Breathable athletic tee.',N'تيشيرت رياضي قابل للتنفس.'),
(5,6,'https://picsum.photos/400?random=284','Adidas Hoodie',N'أديداس هودي','Comfortable hoodie.',N'هودي مريح.'),
(5,5,'https://picsum.photos/400?random=285','Nike Running Shorts',N'شورت جري','Lightweight running shorts.',N'شورت جري خفيف.'),
(5,6,'https://picsum.photos/400?random=286','Adidas Track Pants',N'أديداس بنطال ركض','Warm track pants.',N'بنطال رياضي دافئ.'),
(5,5,'https://picsum.photos/400?random=287','Nike Cap',N'قبعة نايكي','Adjustable cap.',N'قبعة قابلة للتعديل.'),
(5,6,'https://picsum.photos/400?random=288','Adidas Socks',N'أديداس جوارب','Comfort socks.',N'جوارب مريحة.'),
(5,5,'https://picsum.photos/400?random=289','Nike Gym Bag',N'شنطة جم نايكي','Gym carry bag.',N'شنطة حمل للجيم.'),
(5,6,'https://picsum.photos/400?random=290','Adidas Sunglasses',N'نظارات أديداس','Sport sunglasses.',N'نظارات رياضية.'),
(5,5,'https://picsum.photos/400?random=291','Nike Sports Jacket',N'جاكيت رياضي','Lightweight sports jacket.',N'جاكيت رياضي خفيف.'),
(5,6,'https://picsum.photos/400?random=292','Adidas Fitness Tank',N'أديداس تانك','Fitness tank top.',N'قميص رياضي.');
GO

-- Men Clothing (CategoryId = 6)
INSERT INTO Products (category_id, brand_id, default_image_url, name_en, name_ar, description_en, description_ar)
VALUES
(6,5,'https://picsum.photos/400?random=301','Men\'s Leather Jacket',N'جاكيت جلد رجالي','Classic leather jacket.',N'جاكيت جلد كلاسيكي.'),
(6,6,'https://picsum.photos/400?random=302','Men\'s Denim Jeans',N'جينز رجالي','Comfortable denim jeans.',N'جينز مريح.'),
(6,5,'https://picsum.photos/400?random=303','Men\'s Polo Shirt',N'تيشيرت بولو','Casual polo shirt.',N'تيشيرت بولو كاجوال.'),
(6,6,'https://picsum.photos/400?random=304','Men\'s Chinos',N'شينو رجالي','Stylish chinos.',N'شينو أنيق.'),
(6,5,'https://picsum.photos/400?random=305','Men\'s Hoodie',N'هودي رجالي','Warm men's hoodie.',N'هودي رجالي دافئ.'),
(6,6,'https://picsum.photos/400?random=306','Men\'s Dress Shirt',N'قميص رجالي','Smart dress shirt.',N'قميص رسمي.'),
(6,5,'https://picsum.photos/400?random=307','Men\'s Shorts',N'شورت رجالي','Casual shorts.',N'شورت كاجوال.'),
(6,6,'https://picsum.photos/400?random=308','Men\'s Swim Trunks',N'شورت سباحة','Water‑ready swim trunks.',N'شورت للسباحة.'),
(6,5,'https://picsum.photos/400?random=309','Men\'s Sweater',N'سويتر رجالي','Warm sweater.',N'سويتر دافئ.'),
(6,6,'https://picsum.photos/400?random=310','Men\'s Suit',N'بدلة رجالية','Formal men\'s suit.',N'بدلة رسمية.'),
(6,5,'https://picsum.photos/400?random=311','Men\'s Tank Top',N'قميص رجالي بدون أكمام','Casual tank top.',N'قميص بلا أكمام.'),
(6,6,'https://picsum.photos/400?random=312','Men\'s Belt',N'حزام رجالي','Premium leather belt.',N'حزام جلد فخم.');

-- Women Clothing (CategoryId = 7)
INSERT INTO Products (category_id, brand_id, default_image_url, name_en, name_ar, description_en, description_ar)
VALUES
(7,5,'https://picsum.photos/400?random=321','Women\'s Summer Dress',N'فستان صيفي','Light summer dress.',N'فستان صيفي خفيف.'),
(7,6,'https://picsum.photos/400?random=322','Women\'s Blouse',N'بلوزة نسائية','Elegant blouse.',N'بلوزة أنيقة.'),
(7,5,'https://picsum.photos/400?random=323','Women\'s Jeans',N'جينز نسائي','Comfort jeans.',N'جينز مريح.'),
(7,6,'https://picsum.photos/400?random=324','Women\'s Leggings',N'ليغينغ','Comfort leggings.',N'ليغينغ مريح.'),
(7,5,'https://picsum.photos/400?random=325','Women\'s Cardigan',N'كارديغان','Soft cardigan.',N'كارديغان ناعم.'),
(7,6,'https://picsum.photos/400?random=326','Women\'s Skirt',N'تنورة','Stylish skirt.',N'تنورة عصرية.'),
(7,5,'https://picsum.photos/400?random=327','Women\'s Jacket',N'جاكيت نسائي','Trendy jacket.',N'جاكيت أنيق.'),
(7,6,'https://picsum.photos/400?random=328','Women\'s Heels',N'أحذية عالية','Elegant high heels.',N'أحذية بكعب عالي.'),
(7,5,'https://picsum.photos/400?random=329','Women\'s T‑Shirt',N'تيشيرت نسائي','Casual tee.',N'تيشيرت كاجوال.'),
(7,6,'https://picsum.photos/400?random=330','Women\'s Jumpsuit',N'جمبسوت','Chic jumpsuit.',N'جمبسوت أنيق.'),
(7,5,'https://picsum.photos/400?random=331','Women\'s Scarf',N'وشاح','Soft scarf.',N'وشاح ناعم.'),
(7,6,'https://picsum.photos/400?random=332','Women\'s Handbag',N'حقيبة يد','Stylish handbag.',N'حقيبة يدوية.');

-- Accessories (CategoryId = 8)
INSERT INTO Products (category_id, brand_id, default_image_url, name_en, name_ar, description_en, description_ar)
VALUES
(8,5,'https://picsum.photos/400?random=341','Wireless Charger',N'شاحن لاسلكي','Fast wireless charger.',N'شاحن لاسلكي سريع.'),
(8,6,'https://picsum.photos/400?random=342','Bluetooth Speaker',N'سبيكر بلوتوث','Portable bluetooth speaker.',N'سماعة بلوتوث محمولة.'),
(8,5,'https://picsum.photos/400?random=343','Fitness Tracker',N'عداد لياقة','Tracker for fitness activities.',N'جهاز تتبع اللياقة.'),
(8,6,'https://picsum.photos/400?random=344','VR Headset',N'نظارة الواقع الافتراضي','Virtual reality headset.',N'نظارة واقع افتراضي.'),
(8,5,'https://picsum.photos/400?random=345','USB‑C Cable',N'كابل USB‑C','Durable USB‑C cable.',N'كابل USB‑C متين.'),
(8,6,'https://picsum.photos/400?random=346','Portable Power Bank',N'باور بانك','High capacity power bank.',N'باور بانك بسعة عالية.'),
(8,5,'https://picsum.photos/400?random=347','Smart Watch Band',N'حزام ساعة ذكي','Replacement smart watch band.',N'حزام لساعات ذكية.'),
(8,6,'https://picsum.photos/400?random=348','Gaming Mouse',N'ماوس ألعاب','Gaming mouse with RGB.',N'ماوس ألعاب بإضاءة RGB.'),
(8,5,'https://picsum.photos/400?random=349','Laptop Sleeve',N'جراب لابتوب','Protective laptop sleeve.',N'جراب حماية للابتوب.'),
(8,6,'https://picsum.photos/400?random=350','Wireless Earbuds',N'سماعات لاسلكية','True wireless earbuds.',N'سماعات لاسلكية حقيقية.'),
(8,5,'https://picsum.photos/400?random=351','Action Camera',N'كاميرا أكشن','Adventure action camera.',N'كاميرا للمغامرات.'),
(8,6,'https://picsum.photos/400?random=352','Tripod Stand',N'حامل ترايبود','Lightweight tripod stand.',N'حامل ترايبود خفيف.');

GO

-- =============================================
-- 4. Variations (Color, Size, Material)
-- =============================================

INSERT INTO Variations (product_id, name_en, name_ar)
SELECT p.id, v.name_en, v.name_ar
FROM Products p
CROSS APPLY (VALUES
    ('Color', N'اللون'),
    ('Size', N'المقاس'),
    ('Material', N'الخامة')
) v(name_en, name_ar);
GO

-- =============================================
-- 5. VariationOptions (realistic)  
-- =============================================

INSERT INTO VariationOptions (variation_id, value_en, value_ar)
SELECT v.id, vo.value_en, vo.value_ar
FROM Variations v
CROSS APPLY (VALUES
    -- Color realistic
    ('Black', N'أسود'),
    ('White', N'أبيض'),
    ('Red', N'أحمر'),
    ('Blue', N'أزرق'),
    ('Green', N'أخضر')
) vo(value_en, value_ar)
WHERE v.name_en = 'Color';

INSERT INTO VariationOptions (variation_id, value_en, value_ar)
SELECT v.id, vo.value_en, vo.value_ar
FROM Variations v
CROSS APPLY (VALUES
    -- Size realistic
    ('Small', N'صغير'),
    ('Medium', N'متوسط'),
    ('Large', N'كبير'),
    ('XL', N'كبير جدًا')
) vo(value_en, value_ar)
WHERE v.name_en = 'Size';

INSERT INTO VariationOptions (variation_id, value_en, value_ar)
SELECT v.id, vo.value_en, vo.value_ar
FROM Variations v
CROSS APPLY (VALUES
    -- Material realistic
    ('Plastic', N'بلاستيك'),
    ('Metal', N'معدن'),
    ('Leather', N'جلد'),
    ('Cotton', N'قطن')
) vo(value_en, value_ar)
WHERE v.name_en = 'Material';
GO

-- =============================================
-- 6. ProductItems (SKUs generation)
-- =============================================
INSERT INTO ProductItems (product_id, sku)
SELECT DISTINCT p.id,
       CONCAT('P',p.id,'-',REPLACE(c.value_en,' ',''),'-',REPLACE(s.value_en,' ',''),'-',REPLACE(m.value_en,' ','')) AS sku
FROM Products p
JOIN Variations v1 ON v1.product_id=p.id AND v1.name_en='Color'
JOIN Variations v2 ON v2.product_id=p.id AND v2.name_en='Size'
JOIN Variations v3 ON v3.product_id=p.id AND v3.name_en='Material'
JOIN VariationOptions c ON c.variation_id=v1.id
JOIN VariationOptions s ON s.variation_id=v2.id
JOIN VariationOptions m ON m.variation_id=v3.id;
GO

-- =============================================
-- 7. ProductItemVariationOptions
-- =============================================
INSERT INTO ProductItemVariationOptions (variation_option_id, product_item_id)
SELECT vo.id, pi.id
FROM ProductItems pi
JOIN Variations v ON v.product_id=pi.product_id
JOIN VariationOptions vo ON vo.variation_id=v.id
WHERE pi.sku LIKE CONCAT('%',REPLACE(vo.value_en,' ',''),'%');
GO

-- =============================================
-- 8. ProductItemImages (5 per SKU)
-- =============================================
INSERT INTO ProductItemImages (product_item_id, image_url, is_primary)
SELECT pi.id, CONCAT('https://picsum.photos/400?random=',pi.id*10+val.n),
       CASE WHEN val.n=1 THEN 1 ELSE 0 END
FROM ProductItems pi
CROSS APPLY (VALUES(1),(2),(3),(4),(5)) val(n);
GO

-- =============================================
-- 9. People (Sellers)
-- =============================================
INSERT INTO People (first_name,last_name,phone,image_url,gender,date_of_birth)
VALUES
(N'Sara', N'Yousef', '966500001111', 'https://picsum.photos/400?random=401', 2, '1988-07-09'),
(N'Fatima', N'Ali', '966500002222', 'https://picsum.photos/400?random=402', 2, '1991-02-15'),
(N'Omar', N'Hassan', '966500003333', 'https://picsum.photos/400?random=403', 1, '1985-09-30'),
(N'Sara', N'Yousef', '966500001111', 'https://picsum.photos/400?random=401', 2, '1988-07-09'),
(N'Fatima', N'Ali', '966500002222', 'https://picsum.photos/400?random=402', 2, '1991-02-15'),
(N'Omar', N'Hassan', '966500003333', 'https://picsum.photos/400?random=403', 1, '1985-09-30');
GO

-- =============================================
-- 10. Roles
-- =============================================
INSERT INTO Roles (name_en,name_ar) VALUES
('SuperAdmin',N'مشرف عام'),
('Admin',N'مشرف'),
('Customer',N'عميل'),
('Seller',N'بائع');
GO

-- =============================================
-- 11. Users (Customers and Sellers)
-- =============================================
-- Customers
INSERT INTO Users (person_id,email,username,is_confirmed,password)
VALUES
(1,'customer1@test.com','customer1',1,'hashed1'),
(2,'customer2@test.com','customer2',1,'hashed2'),
(3,'customer3@test.com','customer3',1,'hashed3');

-- Sellers
INSERT INTO Users (person_id,email,username,is_confirmed,password)
VALUES
(4,'seller1@test.com','seller1',1,'hashed4'),
(5,'seller2@test.com','seller2',1,'hashed5'),
(6,'seller3@test.com','seller3',1,'hashed6');
GO

INSERT INTO Customers(user_id)
VALUES
(1),
(2),
(3);
GO

-- =============================================
-- 12. UserRoles (Assign roles to Customers and Sellers)
-- =============================================
-- Assuming Role IDs:
-- Customer = 3, Seller = 4
INSERT INTO UserRoles (user_id, role_id) VALUES
-- Customers
(1,3),(2,3),(3,3),
-- Sellers
(4,4),(5,4),(6,4);
GO

-- =============================================
-- 13. Sellers table
-- =============================================
INSERT INTO Sellers (user_id, business_name, website_url)
VALUES
(4,N'Sara Electronics','https://sara-electronics.com'),
(5,N'Fatima Gadgets','https://fatima-gadgets.com'),
(6,N'Omar Tech Store','https://omar-tech.com');
GO




-- =============================================
-- 10. SellerProducts (3 per SKU)
-- =============================================
DECLARE @sid INT,@pid INT,@price DECIMAL(10,2);
DECLARE seller_cur CURSOR FOR SELECT id FROM Sellers;
OPEN seller_cur;
FETCH NEXT FROM seller_cur INTO @sid;
WHILE @@FETCH_STATUS=0
BEGIN
    DECLARE item_cur CURSOR FOR SELECT id FROM ProductItems;
    OPEN item_cur;
    FETCH NEXT FROM item_cur INTO @pid;
    WHILE @@FETCH_STATUS=0
    BEGIN
        SET @price = 100 + @pid*10 + @sid*5;
        INSERT INTO SellerProducts (seller_id, product_item_id, price, stock_quantity, is_active)
        VALUES (@sid,@pid,@price,20+@sid,1);
        FETCH NEXT FROM item_cur INTO @pid;
    END
    CLOSE item_cur;
    DEALLOCATE item_cur;
    FETCH NEXT FROM seller_cur INTO @sid;
END
CLOSE seller_cur;
DEALLOCATE seller_cur;
GO

-- =============================================
-- 11. SellerProductImages (3 per seller product)
-- =============================================
INSERT INTO SellerProductImages (seller_product_id,image_url)
SELECT sp.id, CONCAT('https://picsum.photos/400?random=', sp.id*10 + v.n)
FROM SellerProducts sp
CROSS APPLY (VALUES(1),(2),(3)) v(n);
GO

-- =============================================
-- 15. CustomerWishlist (each customer 5 random products)
-- =============================================
-- Assuming you have 3 customers and 96 products


-- =============================================
-- 16. ProductRatings (each customer rates every product)
-- =============================================
DECLARE @CustomerId INT, @ProductId INT, @Rating INT;

DECLARE customer_cursor CURSOR FOR SELECT id FROM Customers;
OPEN customer_cursor;
FETCH NEXT FROM customer_cursor INTO @CustomerId;

WHILE @@FETCH_STATUS = 0
BEGIN
    DECLARE product_cursor CURSOR FOR SELECT id FROM Products;
    OPEN product_cursor;
    FETCH NEXT FROM product_cursor INTO @ProductId;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Generate random rating 1-5
        SET @Rating = 1 + (ABS(CHECKSUM(NEWID())) % 5);

        INSERT INTO ProductRatings (product_id, customer_id, rating)
        VALUES (@ProductId, @CustomerId, @Rating);

        FETCH NEXT FROM product_cursor INTO @ProductId;
    END

    CLOSE product_cursor;
    DEALLOCATE product_cursor;

    FETCH NEXT FROM customer_cursor INTO @CustomerId;
END

CLOSE customer_cursor;
DEALLOCATE customer_cursor;
GO

-- =============================================
-- 17. SellerRatings (each customer rates every seller)
-- =============================================
DECLARE @CustomerId INT, @SellerId INT, @Rating INT;

DECLARE customer_cursor CURSOR FOR SELECT id FROM Customers;
OPEN customer_cursor;
FETCH NEXT FROM customer_cursor INTO @CustomerId;

WHILE @@FETCH_STATUS = 0
BEGIN
    DECLARE seller_cursor CURSOR FOR SELECT id FROM Sellers;
    OPEN seller_cursor;
    FETCH NEXT FROM seller_cursor INTO @SellerId;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Generate random rating 1-5
        SET @Rating = 1 + (ABS(CHECKSUM(NEWID())) % 5);

        INSERT INTO SellerRatings (seller_id, customer_id, rating)
        VALUES (@SellerId, @CustomerId, @Rating);

        FETCH NEXT FROM seller_cursor INTO @SellerId;
    END

    CLOSE seller_cursor;
    DEALLOCATE seller_cursor;

    FETCH NEXT FROM customer_cursor INTO @CustomerId;
END

CLOSE customer_cursor;
DEALLOCATE customer_cursor;
GO

-- ShippingMethods
USE [Jannara];
GO

INSERT INTO [dbo].[ShippingMethods]
(
    [code],
    [name_en],
    [name_ar],
    [base_price],
    [price_per_kg],
    [free_over],
    [days_min],
    [days_max],
    [is_active],
    [sort_order]
)
VALUES
-- Standard Shipping
(
    'standard-shipping',
    N'Standard Shipping',
    N'شحن عادي',
    35.00,
    5.00,
    200.00,
    3,
    5,
    1,
    1
),

-- Express Shipping
(
    'express-shipping',
    N'Express Shipping',
    N'شحن سريع',
    60.00,
    8.00,
    300.00,
    1,
    2,
    1,
    2
),

-- Same Day Delivery
(
    'same-day-delivery',
    N'Same Day Delivery',
    N'توصيل في نفس اليوم',
    100.00,
    10.00,
    400.00,
    0,
    0,
    1,
    3
),

-- Store Pickup
(
    'store-pickup',
    N'Store Pickup',
    N'الاستلام من المتجر',
    0.00,
    0.00,
    0,
    0,
    0,
    1,
    4
);
GO



 -- Sudan States
USE [Jannara];
GO

INSERT INTO [dbo].[States]
(
    [code],
    [name_en],
    [name_ar],
    [extra_fee_for_shipping]
)
VALUES
('KH', N'Khartoum',        N'الخرطوم',        0.00),
('GZ', N'Al Jazirah',      N'الجزيرة',        10.00),
('GD', N'Gedaref',         N'القضارف',        20.00),
('KA', N'Kassala',         N'كسلا',            25.00),
('RS', N'Red Sea',         N'البحر الأحمر',   30.00),
('NR', N'River Nile',      N'نهر النيل',      15.00),
('NO', N'Northern',        N'الشمالية',       30.00),
('SN', N'Sennar',          N'سنار',           20.00),
('NW', N'White Nile',      N'النيل الأبيض',   20.00),
('NB', N'Blue Nile',       N'النيل الأزرق',   30.00),
('KN', N'North Kordofan',  N'شمال كردفان',    35.00),
('KS', N'South Kordofan',  N'جنوب كردفان',    40.00),
('KW', N'West Kordofan',   N'غرب كردفان',     40.00),
('DN', N'North Darfur',    N'شمال دارفور',    45.00),
('DS', N'South Darfur',    N'جنوب دارفور',    45.00),
('DW', N'West Darfur',     N'غرب دارفور',     45.00),
('DC', N'Central Darfur',  N'وسط دارفور',     45.00),
('DE', N'East Darfur',     N'شرق دارفور',     45.00);
GO




