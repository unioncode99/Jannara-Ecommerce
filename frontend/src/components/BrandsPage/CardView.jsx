import BrandCard from "./BrandCard";
import "./CardView.css";

const CardView = ({ brands, handleDeleteBrand, handleEditBrand }) => {
  return (
    <div className="card-view">
      {brands?.map((brand) => (
        <BrandCard
          handleDeleteBrand={handleDeleteBrand}
          handleEditBrand={handleEditBrand}
          key={brand.id}
          brand={brand}
        />
      ))}
    </div>
  );
};
export default CardView;
