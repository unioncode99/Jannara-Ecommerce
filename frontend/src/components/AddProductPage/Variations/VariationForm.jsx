import { Plus, Tags } from "lucide-react";
import Input from "../../ui/Input";
import Button from "../../ui/Button";
import { useState } from "react";
import "./VariationForm.css";
import { useLanguage } from "../../../hooks/useLanguage";

const VariationForm = ({ addVariation }) => {
  const [nameEn, setNameEn] = useState("");
  const [nameAr, setNameAr] = useState("");

  const { translations } = useLanguage();
  const { variations_header, variation_name_en, variation_name_ar } =
    translations.general.pages.add_product;

  const handleAdd = () => {
    if (addVariation(nameEn, nameAr)) {
      setNameEn("");
      setNameAr("");
    }
  };

  return (
    <header>
      <h3 className="step-title">
        <Tags /> {variations_header}
      </h3>
      <div className="add-variation-inputs">
        <Input
          placeholder={variation_name_en}
          value={nameEn}
          onChange={(e) => setNameEn(e.target.value)}
        />
        <Input
          placeholder={variation_name_ar}
          value={nameAr}
          onChange={(e) => setNameAr(e.target.value)}
        />
        <Button className="btn btn-primary" onClick={handleAdd}>
          <Plus />
        </Button>
      </div>
    </header>
  );
};

export default VariationForm;
