import { useState } from "react";
import SearchableSelect from "../../components/ui/SearchableSelect";
import "./AddEditSellerProduct.css";

// for test
const seedOptions = [
  { id: 1, name: "Apple" },
  { id: 2, name: "Banana" },
  { id: 3, name: "Orange" },
  { id: 4, name: "Mango" },
  { id: 5, name: "Pineapple" },
  { id: 6, name: "Strawberry" },
  { id: 7, name: "Watermelon" },
  { id: 8, name: "Grapes" },
];

const AddEditSellerProduct = () => {
  const [search, setSearch] = useState("");

  const filteredOptions = seedOptions.filter((item) =>
    item.name.toLowerCase().includes(search.toLowerCase())
  );

  return (
    <div>
      AddEditSellerProduct
      <SearchableSelect
        options={filteredOptions}
        inputValue={search}
        onInputChange={setSearch}
        onSelect={(item) => {
          console.log("Selected:", item);
          setSearch(item.name);
        }}
        valueKey="id"
        labelKey="name"
      />
    </div>
  );
};
export default AddEditSellerProduct;
