import { useEffect, useState } from "react";
import SearchableSelect from "../../components/ui/SearchableSelect";
import "./AddEditSellerProduct.css";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useLanguage } from "../../hooks/useLanguage";
import { ArrowLeft, ArrowRight, Save, Upload, X } from "lucide-react";
import Button from "../../components/ui/Button";
import { useDebounce } from "../../hooks/useDebounce";
import { create, read, remove, update } from "../../api/apiWrapper";
import Input from "../../components/ui/Input";
import { isImageValid } from "../../utils/utils";
import { toast } from "../../components/ui/Toast";

const AddEditSellerProduct = () => {
  const [search, setSearch] = useState("");
  const [skuSearch, setSkuSearch] = useState("");
  const debouncedSearch = useDebounce(search);

  const [products, setProducts] = useState([]);
  const [skus, setSkus] = useState([]);

  const [selectedProduct, setSelectedProduct] = useState(null);
  const [selectedSku, setSelectedSku] = useState(null);

  const [stock, setStock] = useState("");
  const [price, setPrice] = useState("");

  const [images, setImages] = useState([]);

  const [isActive, setIsActive] = useState(true);

  const [loadingProducts, setLoadingProducts] = useState(false);
  const [loadingSkus, setLoadingSkus] = useState(false);

  const { translations, language } = useLanguage();
  const [isModeUpdate, setIsModeUpdate] = useState(false);
  const { title, cancel_text, save } = translations.general.pages.add_product;

  const {
    edit_product,
    search_placeholder,
    sku_label,
    price_label,
    stock_label,
    save_product,
    required,
    invalid_price,
    invalid_stock,
    select_sku,
    seller_product_add_success,
    seller_product_add_failed,
    seller_product_update_success,
    seller_product_update_failed,
    seller_product_image_add_success,
    seller_product_image_add_failed,
    seller_product_image_delete_success,
    seller_product_image_delete_failed,
  } = translations.general.pages.seller_products;
  const navigate = useNavigate();
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);

  const { upload_images } = translations.general.pages.add_product;

  const { id } = useParams();

  console.log("id -> ", id);

  useEffect(() => {
    if (id) {
      setIsModeUpdate(true);
      fetchSellerProduct(id);
    } else {
      setIsModeUpdate(false);
      resetForm();
    }
  }, [id]);

  async function fetchProducts() {
    try {
      setLoadingProducts(true);
      const result = await read(
        `products/dropdown?SearchTerm=${debouncedSearch}`,
      );
      console.log("result -> ", result);
      setProducts(result?.data || []);
    } catch (error) {
      console.error("Failed to fetch categories:", error);
    } finally {
      setLoadingProducts(false);
    }
  }

  async function fetchProductItems(productId) {
    try {
      setLoadingSkus(true);
      const result = await read(
        `product-items/dropdown?productId=${productId}`,
      );
      console.log("result -> ", result);
      setSkus(result?.data || []);
    } catch (error) {
      console.error("Failed to fetch categories:", error);
    } finally {
      setLoadingSkus(false);
    }
  }

  useEffect(() => {
    if (!debouncedSearch || debouncedSearch.length < 2) {
      return;
    }

    fetchProducts();
  }, [debouncedSearch]);

  async function handleProductSelect(product) {
    console.log("product -> ", product);
    setSelectedProduct(product);
    setSelectedSku(null);
    setSkuSearch("");
    setSkus([]);
    setImages([]);

    await fetchProductItems(product.id);
  }

  function hanldeProductItemSelect(productItem) {
    console.log("productItem -> ", productItem);
    setSelectedSku(productItem);
    setImages([]);
  }

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!validateFormData()) {
      return;
    }

    if (isModeUpdate) {
      const payload = {
        id: id,
        price: price,
        stockQuantity: stock,
        isActive: true,
      };

      await updateSellerProduct(payload);
    } else {
      const formData = new FormData();

      formData.append("productItemId", selectedSku.id);
      formData.append("stockQuantity", stock);
      formData.append("price", price);

      images.forEach((img) => {
        if (img.isNew) {
          formData.append(`SellerProductImages`, img.file);
        }
      });

      console.log("SEND TO API:", formData);
      await createSellerProduct(formData);
    }
  };

  const validateFormData = () => {
    let newErrors = {};

    if (!selectedSku) {
      newErrors.sku = required;
    } else {
      if (!price || Number(price) <= 0) {
        newErrors.price = invalid_price;
      }
      if (!stock || Number(stock) <= 0) {
        newErrors.stock = invalid_stock;
      }
    }

    setErrors(newErrors);

    return Object.keys(newErrors).length === 0; // true = valid
  };

  const resetForm = () => {
    setSelectedProduct(null);
    setSelectedSku(null);
    setStock("");
    setPrice("");
    setErrors({});
  };

  const addImages = (files) => {
    const validFiles = Array.from(files)
      .filter((file) => isImageValid(file))
      .map((file) => ({ file, isNew: true }));

    if (isModeUpdate) {
      const formData = new FormData();
      formData.append("sellerProductId", id);

      for (const file of validFiles) {
        formData.append("Images", file?.file);
      }
      addSellerProductImages(formData);
    } else {
      setImages((prev) => [...prev, ...validFiles]);
    }
  };

  const removeImage = (index) => {
    const updated = [...images];
    const removed = updated.splice(index, 1)[0];

    if (!removed?.isNew && removed?.id) {
      deleteSellerProductImage(removed.id);
    } else {
      setImages(updated);
    }
  };

  const getImagePrview = (img) => {
    if (img.isNew && img.file && img.file instanceof File) {
      return URL.createObjectURL(img.file);
    }
    if (!img.isNew && img.url) {
      return img.url;
    }

    return "";
  };

  async function createSellerProduct(payload) {
    try {
      const result = await create(`seller-products`, payload);

      console.log("result -> ", result);

      if (translations.general.server_messages[result?.message?.message]) {
        toast.show(
          translations.general.server_messages[result?.message?.message],
          "success",
        );
      } else {
        toast.show(seller_product_add_success, "success");
      }
      navigate("/seller-products");
    } catch (error) {
      console.error(error);
      if (translations.general.server_messages[error.message]) {
        toast.show(
          translations.general.server_messages[error.message],
          "error",
        );
      } else {
        toast.show(seller_product_add_failed, "error");
      }
    }
  }

  async function updateSellerProduct(payload) {
    try {
      const result = await update(`seller-products/${id}`, payload);

      console.log("result -> ", result);

      if (translations.general.server_messages[result?.message?.message]) {
        toast.show(
          translations.general.server_messages[result?.message?.message],
          "success",
        );
      } else {
        toast.show(seller_product_update_success, "success");
      }
      navigate("/seller-products");
    } catch (error) {
      console.error(error);
      if (translations.general.server_messages[error.message]) {
        toast.show(
          translations.general.server_messages[error.message],
          "error",
        );
      } else {
        toast.show(seller_product_update_failed, "error");
      }
    }
  }

  async function fetchSellerProduct(id) {
    try {
      setLoading(true);
      const result = await read(`seller-products/edit/${id}`);
      console.log("result -> ", result);

      setSelectedProduct({
        id: result.data.productId,
        nameEn: result.data.productNameEn,
        nameAr: result.data.productNameAr,
      });

      await fetchProductItems(result.data.productId);

      setSearch(result.data.productNameEn);
      setSkuSearch(result.data.sku);

      setSelectedSku({
        id: result.data.productItemId,
        sku: result.data.sku,
      });

      setPrice(result.data.price);
      setStock(result.data.stockQuantity);

      setImages(
        result.data.sellerProductImages.map((img) => ({
          id: img.id,
          url: img.imageUrl,
          isNew: false,
        })) || [],
      );
    } catch (error) {
      console.error("Failed to fetch product", error);
      // toast.show("Failed to load product", "error");
    } finally {
      setLoading(false);
    }
  }

  async function addSellerProductImages(payload) {
    try {
      const result = await create(`seller-product-images`, payload);

      console.log("result -> ", result);

      const newImages = result?.map((img) => ({
        id: img.id,
        url: img.imageUrl,
        isNew: false,
      }));

      setImages((prev) => [...prev, ...newImages]);

      if (translations.general.server_messages[result?.message?.message]) {
        toast.show(
          translations.general.server_messages[result?.message?.message],
          "success",
        );
      } else {
        toast.show(seller_product_image_add_success, "success");
      }
    } catch (error) {
      console.error(error);
      if (translations.general.server_messages[error.message]) {
        toast.show(
          translations.general.server_messages[error.message],
          "error",
        );
      } else {
        toast.show(seller_product_image_add_failed, "error");
      }
    }
  }

  async function deleteSellerProductImage(imageId) {
    try {
      const result = await remove(`seller-product-images/${imageId}`);

      console.log("result -> ", result);

      setImages((prev) => prev.filter((image) => image.id !== imageId));

      if (translations.general.server_messages[result?.message?.message]) {
        toast.show(
          translations.general.server_messages[result?.message?.message],
          "success",
        );
      } else {
        toast.show(seller_product_image_delete_success, "success");
      }
    } catch (error) {
      console.error(error);
      if (translations.general.server_messages[error.message]) {
        toast.show(
          translations.general.server_messages[error.message],
          "error",
        );
      } else {
        toast.show(seller_product_image_delete_failed, "error");
      }
    }
  }

  return (
    <div className="add-edit-seller-product-container">
      <header>
        <div>
          <Link to="/seller-products" className="back-home-link">
            {language == "en" ? <ArrowLeft /> : <ArrowRight />}
          </Link>
          <h2>{isModeUpdate ? edit_product : title}</h2>
        </div>
        <div>
          <Button
            onClick={() => navigate(`/seller-products`)}
            className="cancel-btn"
          >
            {cancel_text}
          </Button>
          <Button className="btn btn-primary save-btn">
            <Save />
            <span>{save}</span>
          </Button>
        </div>
      </header>

      <form
        onSubmit={handleSubmit}
        className="form add-edit-seller-product-form"
      >
        <h3>{isModeUpdate ? edit_product : title}</h3>
        {/* PRODUCT SEARCH */}
        <SearchableSelect
          options={products}
          inputValue={search}
          onInputChange={setSearch}
          placeholder={search_placeholder}
          onSelect={handleProductSelect}
          loading={loadingProducts}
          valueKey="id"
          labelKey={language == "en" ? "nameEn" : "nameAr"}
          disabled={isModeUpdate}
        />
        {/* SKU SELECT */}
        {selectedProduct && (
          <>
            <SearchableSelect
              options={skus}
              inputValue={skuSearch}
              onInputChange={setSkuSearch}
              placeholder={select_sku}
              onSelect={hanldeProductItemSelect}
              loading={loadingSkus}
              valueKey="id"
              labelKey="sku"
              lang={language}
              disabled={isModeUpdate}
            />
            {errors.sku && <div className="form-alert">{errors.sku}</div>}
          </>
        )}
        {selectedSku && (
          <>
            <Input
              label={price_label}
              name="price"
              type="number"
              placeholder={price_label}
              value={price}
              onChange={(e) => setPrice(e.target.value)}
              errorMessage={errors.price}
            />
            <Input
              label={stock_label}
              name="stock"
              type="number"
              placeholder={stock_label}
              value={stock}
              onChange={(e) => setStock(e.target.value)}
              errorMessage={errors.stock}
            />

            <>
              <label className="upload-product-image-container">
                <Upload className="upload-icon" />
                <span>{upload_images}</span>
                <input
                  type="file"
                  accept="image/*"
                  multiple
                  style={{ display: "none" }}
                  onChange={(e) => addImages(e.target.files)}
                />
              </label>

              <div className="sku-images">
                {images?.map((img, index) => (
                  <div key={index} className={`product-preview-container`}>
                    <img
                      src={getImagePrview(img)}
                      alt="Product Image"
                      className="product-img"
                    />
                    <button type="button" onClick={() => removeImage(index)}>
                      <X />
                    </button>
                  </div>
                ))}
              </div>
            </>
          </>
        )}

        <div className="submit-container">
          <Button type="submit" className="btn btn-primary">
            {save_product}
          </Button>
        </div>
      </form>
    </div>
  );
};
export default AddEditSellerProduct;
