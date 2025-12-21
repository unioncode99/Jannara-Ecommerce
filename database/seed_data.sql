USE [Jannara]
GO

-- Roles
INSERT INTO Roles (name_en, name_ar)
VALUES
    ('SuperAdmin', N'مشرف عام'),
    ('Admin', N'مشرف'),
    ('Customer', N'عميل'),
    ('Seller', N'بائع');

-- Brands
INSERT INTO [dbo].[Brands]
(
    [name_en],
    [name_ar],
    [logo_url],
    [website_url],
    [description_en],
    [description_ar],
    [created_at],
    [updated_at]
)
VALUES
-- Samsung
(
    'Samsung',
    N'سامسونج',
    'https://upload.wikimedia.org/wikipedia/commons/2/24/Samsung_Logo.svg',
    'https://www.samsung.com',
    'Samsung is a global leader in electronics, smartphones, and home appliances.',
    N'سامسونج شركة عالمية رائدة في الإلكترونيات والهواتف الذكية والأجهزة المنزلية.',
    GETDATE(),
    GETDATE()
),

-- Apple
(
    'Apple',
    N'آبل',
    'https://upload.wikimedia.org/wikipedia/commons/f/fa/Apple_logo_black.svg',
    'https://www.apple.com',
    'Apple designs consumer electronics, software, and services including iPhone and Mac.',
    N'تصمم آبل الأجهزة الإلكترونية الاستهلاكية والبرمجيات والخدمات مثل آيفون وماك.',
    GETDATE(),
    GETDATE()
),

-- Sony
(
    'Sony',
    N'سوني',
    'https://upload.wikimedia.org/wikipedia/commons/2/29/Sony_logo.svg',
    'https://www.sony.com',
    'Sony is a multinational company known for electronics, gaming, and entertainment.',
    N'سوني شركة متعددة الجنسيات معروفة بالإلكترونيات والألعاب والترفيه.',
    GETDATE(),
    GETDATE()
),

-- LG
(
    'LG',
    N'إل جي',
    'https://upload.wikimedia.org/wikipedia/commons/2/20/LG_symbol.svg',
    'https://www.lg.com',
    'LG specializes in home appliances, consumer electronics, and smart solutions.',
    N'تتخصص إل جي في الأجهزة المنزلية والإلكترونيات الاستهلاكية والحلول الذكية.',
    GETDATE(),
    GETDATE()
);
GO

-- ProductCategories
INSERT INTO [dbo].[ProductCategories]
(
    [parent_category_id],
    [name_en],
    [name_ar],
    [description_en],
    [description_ar]
)
VALUES
(NULL, 'Electronics', N'الإلكترونيات',
 'Electronic devices and gadgets',
 N'الأجهزة الإلكترونية والأدوات الذكية'),

(NULL, 'Mobile Phones', N'الهواتف الذكية',
 'Smartphones from global brands',
 N'الهواتف الذكية من العلامات التجارية العالمية'),

(NULL, 'Computers & Laptops', N'أجهزة الكمبيوتر واللابتوب',
 'Desktops, laptops, and accessories',
 N'أجهزة الكمبيوتر المكتبية والمحمولة وملحقاتها'),

(NULL, 'Home Appliances', N'الأجهزة المنزلية',
 'Appliances for home and kitchen use',
 N'أجهزة المنزل والمطبخ'),

(NULL, 'Fashion', N'الأزياء',
 'Clothing, shoes, and accessories',
 N'الملابس والأحذية والإكسسوارات'),

(NULL, 'Men Clothing', N'ملابس رجالية',
 'Clothing designed for men',
 N'ملابس مخصصة للرجال'),

(NULL, 'Women Clothing', N'ملابس نسائية',
 'Clothing designed for women',
 N'ملابس مخصصة للنساء'),

(NULL, 'Accessories', N'الإكسسوارات',
 'Tech and fashion accessories',
 N'إكسسوارات تقنية وعصرية');
GO


-- Products
INSERT INTO [dbo].[Products]
           ([category_id]
           ,[brand_id]
           ,[default_image_url]
           ,[name_en]
           ,[name_ar])
     VALUES
           (1
           ,1
           ,'https://picsum.photos/400/400?random=1'
           ,'iPhone 15'
           ,'iPhone 15'),
		    (1
           ,1
           ,'https://picsum.photos/400/400?random=2'
           ,'Samsung Galaxy S24'
           ,'Samsung Galaxy S24'),
		              (1
           ,1
           ,'https://picsum.photos/400/400?random=3'
           ,'MacBook Pro M3'
           ,'MacBook Pro M3'),
		              (1
           ,1
           ,'https://picsum.photos/400/400?random=4'
           ,'Sony WH-1000XM5'
           ,'Sony WH-1000XM5'),
		              (1
           ,1
           ,'https://picsum.photos/400/400?random=5'
           ,'Apple Watch Series 9'
           ,'Apple Watch Series 9'),
		              (1
           ,1
           ,'https://picsum.photos/400/400?random=6'
           ,'iPad Pro 12.9'
           ,'iPad Pro 12.9');
		   GO

-- Variations
INSERT INTO [dbo].[Variations]
(
    [product_id],
    [name_en],
    [name_ar]
)
VALUES
-- Color variation
(1, 'Color', N'اللون'),

-- Storage variation
(1, 'Storage', N'سعة التخزين'),

-- Size variation (for clothing or accessories)
(1, 'Size', N'المقاس');
GO

-- VariationOptions
INSERT INTO [dbo].[VariationOptions]
(
    [variation_id],
    [value_en],
    [value_ar]
)
VALUES
(1, 'Black',  N'أسود'),
(1, 'White',  N'أبيض'),
(1, 'Blue',   N'أزرق'),
(2, '128 GB', N'128 جيجابايت'),
(2, '256 GB', N'256 جيجابايت'),
(2, '512 GB', N'512 جيجابايت'),
(3, 'Small',  N'صغير'),
(3, 'Medium', N'متوسط'),
(3, 'Large',  N'كبير');
GO

-- ProductItems
INSERT INTO [dbo].[ProductItems]
(
    [product_id],
    [sku]
)
VALUES
(1, 'SAM-GAL-BLK-128'),
(1, 'SAM-GAL-BLK-256'),
(1, 'SAM-GAL-WHT-128'),
(1, 'SAM-GAL-BLU-256');
GO

-- ProductItemVariationOptions
INSERT INTO [dbo].[ProductItemVariationOptions]
(
    [variation_option_id],
    [product_item_id]
)
VALUES
-- Black 128 GB -- Small
(1, 1), -- Black
(4, 1), -- 128 GB
(7, 1), -- Small

-- Black 256 GB -- Small
(1, 2),	-- Black
(5, 2), -- 256 GB
(7, 2), -- Small

-- White 128 GB -- Small
(2, 3), -- White
(4, 3), -- 128 GB
(7, 3), -- Small

-- Blue 256 GB -- Small
(3, 4), -- Blue
(5, 4),  -- 256 GB
(5, 4); -- -- Small
GO

-- ProductItemImages
INSERT INTO [dbo].[ProductItemImages]
(
    [product_item_id],
    [image_url],
    [is_primary]
)
VALUES
(1, 'https://picsum.photos/400/400?random=1', 1),
(1, 'https://picsum.photos/400/400?random=1',  0),
(2, 'https://picsum.photos/400/400?random=1',   1),
(3, 'https://picsum.photos/400/400?random=1', 1),
(4, 'https://picsum.photos/400/400?random=1',  1);
GO
-- People
INSERT INTO [dbo].[People]
(
    [first_name],
    [last_name],
    [phone],
    [image_url],
    [gender],
    [date_of_birth]
)
VALUES
(N'Mohammed', N'Alfatih', '966501234567', 'https://picsum.photos/400/400?random=9', 1, '1990-05-12'),
(N'Muzamil',  N'Alfatih',  '966509876543', 'https://picsum.photos/400/400?random=10',  2, '1995-08-22');
GO
-- Users
INSERT INTO [dbo].[Users]
(
    [person_id],
    [email],
    [username],
    [is_confirmed],
    [password]
)
VALUES
(1, 'mohammedalfatih606@gmail.com', 'mohammed_store', 1, 'hashed_password_123'),
(2, 'mohammedalfatih620@gmail.com',  'muzamil_store',  1, 'hashed_password_456');
GO

-- User Rolse
INSERT INTO UserRoles(user_id, role_id)
VALUES
(1, 4),
(2, 4);
GO

-- Sellers
INSERT INTO [dbo].[Sellers]
(
    [user_id],
    [business_name],
    [website_url]
)
VALUES
(1, N'Mohammed Electronics', 'https://ahmed-electronics.com'),
(2, N'Muzamil Tech Store',   'https://saratech.com');
GO
-- SellerProducts
INSERT INTO [dbo].[SellerProducts]
(
    [seller_id],
    [product_item_id],
    [price],
    [stock_quantity],
    [is_active]
)
VALUES
(1, 1, 3499.00, 25, 1),
(1, 2, 3699.00, 15, 1),
(2, 1, 3450.00, 10, 1);
GO
-- SellerProductImages
INSERT INTO [dbo].[SellerProductImages]
(
    [seller_product_id],
    [image_url]
)
VALUES
(1, 'https://picsum.photos/400/400?random=11'),
(1, 'https://picsum.photos/400/400?random=12'),
(2, 'https://picsum.photos/400/400?random=13'),
(3, 'https://picsum.photos/400/400?random=14');
GO
