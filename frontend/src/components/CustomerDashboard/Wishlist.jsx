import { Link } from "react-router-dom";
import { useLanguage } from "../../hooks/useLanguage";
import "./Wishlist.css";
import { formatMoney } from "../../utils/utils";

const Wishlist = ({ wishlist }) => {
  const { language, translations } = useLanguage();
  const { wishlist: wishlist_label, view_all } =
    translations.general.pages.customer_dashboard;
  return (
    <div className="customer-wishlist-container">
      <h3>
        <span>{wishlist_label}</span>
        <Link to="/favorites" className="view-all-orders-link">
          <small>{view_all}</small>
        </Link>
      </h3>
      {wishlist?.map((item) => (
        <Link to={`/product/${item?.publicId}`}>
          <div className="wishlist-item">
            <div className="wishlist-item-image-container">
              <img src={item.productImageUrl} alt="Product Image" />
            </div>
            <div>
              <p>
                {language == "en" ? item.productNameEn : item.productNameAr}
              </p>
              <p>{formatMoney(item.minPrice)}</p>
            </div>
          </div>
        </Link>
      ))}
    </div>
  );
};
export default Wishlist;
