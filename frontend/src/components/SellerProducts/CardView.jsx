import ProductCard from "./ProductCard";

const CardView = ({ products, handleEditProduct, handleDeleteProduct }) => {
  return (
    <div className="card-view">
      {products?.map((product) => (
        <ProductCard
          key={product.id}
          product={product}
          handleEditProduct={handleEditProduct}
          handleDeleteProduct={handleDeleteProduct}
        />
      ))}
    </div>
  );
};
export default CardView;
