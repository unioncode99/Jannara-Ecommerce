import "./CardView.css";
import ProductCard from "./ProductCard";
const CardView = ({ products }) => {
  return (
    <div className="card-view">
      {products?.map((product) => (
        <ProductCard key={product.id} product={product} />
      ))}
    </div>
  );
};
export default CardView;
