import { useParams } from "react-router-dom";

const ProductPage = () => {
  const { publicId } = useParams();
  console.log("publicId", publicId);
  return <div>ProductPage</div>;
};
export default ProductPage;
