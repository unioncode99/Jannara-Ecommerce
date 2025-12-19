import { Search } from "lucide-react";
import Input from "../components/ui/Input";
import "./Home.css";
import ProductCategoriesDropdown from "../components/ProductCategoriesDropdown";
import { useEffect, useState } from "react";
import { useLanguage } from "../hooks/useLanguage";
import ProductList from "../components/ProductList";
import ProductSortingFiltersDropdown from "../components/ProductSortingFiltersDropdown";
import Button from "../components/ui/Button";
import Pagination from "../components/ui/Pagination";

const Home = () => {
  const [searchText, setSearchText] = useState("");
  const [debouncedSearch, setDebouncedSearch] = useState("");
  const [selectProductCategoryId, setSelectProductCategoryId] = useState(-1);
  const [sortingTerm, setSortingTerm] = useState("");
  const { translations } = useLanguage();

  const handleSearchInputChange = (e) => {
    console.log("search -> ", e.target.value);
    setSearchText(e.target.value);
  };

  useEffect(() => {
    const timer = setTimeout(() => {
      setDebouncedSearch(searchText);
    }, 500);

    return () => clearTimeout(timer);
  }, [searchText]);

  const handleProductCategoryChange = (e) => {
    console.log("category -> ", e.target.value);
    setSelectProductCategoryId(e.target.value);
  };

  const handleSortingTermChange = (e) => {
    console.log("Sorting Term -> ", e.target.value);
    setSortingTerm(e.target.value);
  };

  return (
    <>
      <div className="product-filter-container">
        <Input
          label="Product Search"
          name="productSearch"
          placeholder={
            translations.general.pages.home.search_product_placeholder
          }
          icon={<Search />}
          type="search"
          value={searchText}
          onChange={handleSearchInputChange}
        />
        <ProductCategoriesDropdown
          value={selectProductCategoryId}
          onChange={handleProductCategoryChange}
        />
        <ProductSortingFiltersDropdown
          value={sortingTerm}
          onChange={handleSortingTermChange}
        />
      </div>
      <div></div>
      <ProductList
        searchTerm={debouncedSearch}
        categoryId={selectProductCategoryId}
        sortingTerm={sortingTerm}
      />
    </>
  );
};
export default Home;
