import { Search } from "lucide-react";
import Input from "../ui/Input";
import OrdersSortingFilter from "./OrdersSortingFilter";
import { useLanguage } from "../../hooks/useLanguage";

const FilterContainer = ({
  searchText,
  handleSearchInputChange,
  sortingTerm,
  handleSortingTermChange,
}) => {
  const { translations } = useLanguage();
  const { search_order_placeholder } =
    translations.general.pages.customer_orders;
  return (
    <div className="customer-orders-filter-container">
      <Input
        label="Product Search"
        name="productSearch"
        placeholder={search_order_placeholder}
        icon={<Search />}
        type="search"
        value={searchText}
        onChange={handleSearchInputChange}
      />
      <OrdersSortingFilter
        value={sortingTerm}
        onChange={handleSortingTermChange}
      />
    </div>
  );
};
export default FilterContainer;
