import { Heart } from "lucide-react";
import ProductList from "../components/ProductList";

const FavoritesPage = () => {
  return (
    <div>
      <h1>
        <Heart /> Favorites{" "}
      </h1>
      <ProductList />
    </div>
  );
};
export default FavoritesPage;
