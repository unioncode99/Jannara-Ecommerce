import { useEffect, useState } from "react";
import SearchableSelect from "../../components/ui/SearchableSelect";
import "./AddEditSellerProduct.css";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useLanguage } from "../../hooks/useLanguage";
import { ArrowLeft, ArrowRight, Save } from "lucide-react";
import Button from "../../components/ui/Button";
import { useDebounce } from "../../hooks/useDebounce";
import { read } from "../../api/apiWrapper";
import Input from "../../components/ui/Input";

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
  } = translations.general.pages.seller_products;
  const navigate = useNavigate();
  const [errors, setErrors] = useState({});

  const { id } = useParams();

  console.log("id -> ", id);

  useEffect(() => {
    if (id) {
      setIsModeUpdate(true);
      // TODO: fetch product by id for edit
    } else {
      setIsModeUpdate(false);
      resetForm();
    }
  }, [id]);

  async function fetchProducts() {
    try {
      setLoadingProducts(true);
      const result = await read(
        `products/dropdown?SearchTerm=${debouncedSearch}`
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
        `product-items/dropdown?productId=${productId}`
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

    await fetchProductItems(product.id);
  }

  function hanldeProductItemSelect(productItem) {
    console.log("productItem -> ", productItem);
    setSelectedSku(productItem);
  }

  const handleSubmit = (e) => {
    e.preventDefault();

    if (!validateFormData()) {
      return;
    }

    const payload = {
      productItemId: selectedSku.id,
      stockQuantity: Number(stock),
      price: Number(price),
    };

    // TODO:
    // await createSellerProduct(payload)
    // navigate("/seller-products");

    console.log("SEND TO API:", payload);
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
