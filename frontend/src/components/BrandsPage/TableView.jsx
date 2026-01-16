import { Pencil, Trash2 } from "lucide-react";
import Table from "../../components/ui/Table";

const TableView = ({ brands, handleDeleteBrand, handleEditBrand }) => {
  return (
    <div className="table-view">
      <Table
        headers={[
          "name_en",
          "name_ar",
          "description_en",
          "description_ar",
          "actions",
        ]}
        data={brands.map((brand) => ({
          name_en: brand.nameEn,
          name_ar: brand.nameAr,
          description_en: brand.descriptionEn,
          description_ar: brand.descriptionAr,
          Actions: (
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
          ),
        }))}
      />
    </div>
  );
};
export default TableView;
