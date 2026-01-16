import { Pencil, Tags, Trash2 } from "lucide-react";
import { useLanguage } from "../../hooks/useLanguage";
import "./BrandCard.css";

const BrandCard = ({ brand, handleDeleteBrand, handleEditBrand }) => {
  const { language } = useLanguage();

  return (
    <div className="brand-card">
      <h3>
        <span>
          <Tags />
        </span>
        {language == "en" ? brand.nameEn : brand.nameAr}
      </h3>
      <p>{language == "en" ? brand.descriptionEn : brand.descriptionAr}</p>
      {/* <p>{brand.websiteUrl}</p> */}
      {/* <img src={brand.logoUrl} alt="" /> */}
      <div className="brand-actions-btn">
        <button
          onClick={() => handleEditBrand(brand)}
          className="edit-brand-btn"
        >
          <Pencil />
        </button>
        <button
          onClick={() => handleDeleteBrand(brand)}
          className="delete-brand-btn"
        >
          <Trash2 />
        </button>
      </div>
    </div>
  );
};
export default BrandCard;
