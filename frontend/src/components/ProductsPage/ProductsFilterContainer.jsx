import { Search, XCircle } from "lucide-react";
import Input from "../ui/Input";
import "./ProductsFilterContainer.css";
import ProductsSortingFilter from "./ProductsSortingFilter";
import BrandsDropdown from "../../components/BrandsDropdown";
import ProductCategoriesDropdown from "../../components/ProductCategoriesDropdown";
import { useLanguage } from "../../hooks/useLanguage";
import Button from "../ui/Button";

const ProductsFilterContainer = ({
  searchText,
  handleSearchInputChange,
  sortingTerm,
  handleSortingTermChange,
  productCategoryId,
  handleProductCategoryIdChange,
  brandId,
  handleBrandIdChange,
  handleClearFilters,
}) => {
  const { translations } = useLanguage();
  const { search_label, search_product_placeholder, clear_filters } =
    translations.general.pages.products;

  return (
    <div className="products-filter-container">
      <Input
        label={search_label}
        name="userSearch"
        placeholder={search_product_placeholder}
        icon={<Search />}
        type="search"
        value={searchText}
        onChange={handleSearchInputChange}
      />
      <ProductsSortingFilter
        value={sortingTerm}
        onChange={handleSortingTermChange}
      />
      <BrandsDropdown
        name="brandId"
        value={brandId}
        onChange={handleBrandIdChange}
      />
      <ProductCategoriesDropdown
        name="productCategoryId"
        value={productCategoryId}
        onChange={handleProductCategoryIdChange}
      />
      {(searchText || sortingTerm || brandId || productCategoryId) && (
        <div className="clear-filters-container">
          <Button
            className="btn btn-primary clear-filters-btn"
            onClick={handleClearFilters}
          >
            <XCircle /> {clear_filters}
          </Button>
        </div>
      )}
    </div>
  );
};
export default ProductsFilterContainer;
