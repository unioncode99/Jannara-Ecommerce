import { useState, useEffect, useMemo } from "react";
import "./ProductGallery.css";

function ProductGallery({ selectedItem, selectedSellerProductId }) {
  // State for main image
  const [mainImage, setMainImage] = useState("");

  // Combine and deduplicate all images
  const galleryImages = useMemo(() => {
    if (!selectedItem) return [];

    // Product item images
    const itemImages = (selectedItem.productItemImages || []).map((img) => ({
      id: `item-${img.productItemImageId}`,
      imageUrl: img.imageUrl,
      isDefault: img.isDefault || false,
    }));

    // Seller product images
    const sellerImages =
      selectedItem.sellerProducts
        ?.find((s) => s.sellerProductId === selectedSellerProductId)
        ?.sellerProductImages.map((img) => ({
          id: `seller-${img.sellerProductImageId}`,
          imageUrl: img.imageUrl,
          isDefault: false, // seller images don’t have default
        })) || [];

    // Deduplicate by imageUrl
    const uniqueImagesMap = new Map(
      [...itemImages, ...sellerImages].map((img) => [img.imageUrl, img])
    );

    return [...uniqueImagesMap.values()];
  }, [selectedItem, selectedSellerProductId]);

  // Set main image to default when selectedItem changes
  useEffect(() => {
    if (!selectedItem || !galleryImages.length) return;

    const defaultImage = galleryImages.find((img) => img.isDefault);
    setMainImage(defaultImage?.imageUrl || galleryImages[0].imageUrl);
  }, [selectedItem, galleryImages]);

  if (!galleryImages.length) return null;

  return (
    <div className="product-gallery">
      <div className="gallery-main">
        <img src={mainImage} alt="Product Item Default Image" />
      </div>
      <div className="gallery-thumbs">
        {galleryImages.map((img) => (
          <button
            key={img.id}
            className={`gallery-thumb ${
              mainImage === img.imageUrl ? "active" : ""
            }`}
            onClick={() => setMainImage(img.imageUrl)}
          >
            <img src={img.imageUrl} alt="Thumb" />
          </button>
        ))}
      </div>
    </div>
  );
}

export default ProductGallery;
