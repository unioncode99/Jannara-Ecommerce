import { useEffect, useState } from "react";
import { useLanguage } from "../../hooks/useLanguage";
import { create, read, remove, update } from "../../api/apiWrapper";
import SpinnerLoader from "../../components/ui/SpinnerLoader";
import { Plus, Tags } from "lucide-react";
import Button from "../../components/ui/Button";
import ViewSwitcher from "../../components/CustomerOrders/ViewSwitcher";
import CardView from "../../components/BrandsPage/CardView";
import TableView from "../../components/BrandsPage/TableView";
import AddEditBrandModal from "../../components/BrandsPage/AddEditBrandModal";
import ConfirmModal from "../../components/ui/ConfirmModal";
import "./BrandsPage.css";
import { toast } from "../../components/ui/Toast";

const BrandsPage = () => {
  const [view, setView] = useState("card"); // 'table' or 'card'
  const [brands, setBrands] = useState([]);
  const [loading, setLoading] = useState(false);
  const [isAddEditBrandModalOpen, setIsAddEditBrandModalOpen] = useState(false);
  const [isDeleteBrandConfirmModalOpen, setIsDeleteBrandConfirmModalOpen] =
    useState(false);
  const [selectedBrand, setSelectedBrand] = useState(null);
  const { translations } = useLanguage();

  const fetchBrands = async () => {
    try {
      setLoading(true);
      const data = await read("brands");
      console.log("data", data);
      setBrands(data);
    } catch (error) {
      console.error("Failed to Brands:", error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchBrands();
  }, []);

  function handleDeleteBrand(brand) {
    setSelectedBrand(brand);
    setIsDeleteBrandConfirmModalOpen(true);
    console.log("brand -> ", brand);
  }
  function handleEditBrand(brand) {
    setSelectedBrand(brand);
    setIsAddEditBrandModalOpen(true);
    console.log("brand -> ", brand);
  }

  function handleAddBrand() {
    setSelectedBrand(null);
    setIsAddEditBrandModalOpen(true);
  }

  const closeModal = () => {
    setIsAddEditBrandModalOpen(false);
    setIsDeleteBrandConfirmModalOpen(false);
    setSelectedBrand(null);
  };

  async function addEditBrand(payload) {
    console.log("payload", payload);

    if (selectedBrand) {
      editBrand(payload);
    } else {
      addBrand(payload);
    }
  }

  async function addBrand(payload) {
    try {
      const result = await create(`brands`, payload);

      const newBrand = result.data || result;

      console.log("result -> ", result);
      console.log("newBrand -> ", newBrand);

      setBrands((prev) => [...prev, newBrand]);

      if (translations.general.server_messages[result?.message?.message]) {
        toast.show(
          translations.general.server_messages[result?.message?.message],
          "success"
        );
      } else {
        toast.show("product_variation_option_create_success", "success");
      }
    } catch (error) {
      console.error(error);
      if (translations.general.server_messages[error.message]) {
        toast.show(
          translations.general.server_messages[error.message],
          "error"
        );
      } else {
        toast.show("product_variation_option_create_failed", "error");
      }
    } finally {
      closeModal();
    }
  }

  async function editBrand(payload) {
    try {
      const result = await update(`brands/${selectedBrand.id}`, payload);
      console.log("result -> ", result);

      const updatedBrand = result.data || result;

      setBrands((prev) =>
        prev.map((brand) =>
          brand.id === updatedBrand.id ? updatedBrand : brand
        )
      );

      if (translations.general.server_messages[result?.message?.message]) {
        toast.show(
          translations.general.server_messages[result?.message?.message],
          "success"
        );
      } else {
        toast.show("product_variation_update_success", "success");
      }
    } catch (error) {
      console.error(error);
      if (translations.general.server_messages[error.message]) {
        toast.show(
          translations.general.server_messages[error.message],
          "error"
        );
      } else {
        toast.show("product_variation_update_failed", "error");
      }
    } finally {
      closeModal();
    }
  }

  async function deleteBrand() {
    try {
      const result = await remove(`brands/${selectedBrand.id}`);
      console.log("result -> ", result);

      setBrands((prev) => prev.filter((brand) => brand.id != selectedBrand.id));

      if (translations.general.server_messages[result?.message?.message]) {
        toast.show(
          translations.general.server_messages[result?.message?.message],
          "success"
        );
      } else {
        toast.show("product_variation_delete_success", "success");
      }
    } catch (error) {
      console.error(error);
      if (translations.general.server_messages[error.message]) {
        toast.show(
          translations.general.server_messages[error.message],
          "error"
        );
      } else {
        toast.show("product_variation_delete_failed", "error");
      }
    } finally {
      closeModal();
    }
  }

  if (loading) {
    return (
      <div className="loader-container">
        <SpinnerLoader />
      </div>
    );
  }

  return (
    <div className="brand-page-container">
      <header>
        <h1>
          <Tags /> {"title"}
        </h1>
        <Button onClick={handleAddBrand} className="btn btn-primary">
          <Plus /> {"add_category"}
        </Button>
      </header>
      <ViewSwitcher view={view} setView={setView} />
      {view == "card" && (
        <CardView
          brands={brands}
          handleDeleteBrand={handleDeleteBrand}
          handleEditBrand={handleEditBrand}
        />
      )}
      {view == "table" && (
        <TableView
          brands={brands}
          handleDeleteBrand={handleDeleteBrand}
          handleEditBrand={handleEditBrand}
        />
      )}
      <AddEditBrandModal
        show={isAddEditBrandModalOpen}
        onClose={closeModal}
        onConfirm={addEditBrand}
        brand={selectedBrand}
        brands={brands}
      />
      <ConfirmModal
        show={isDeleteBrandConfirmModalOpen}
        onClose={() => closeModal()}
        onConfirm={() => deleteBrand()}
        title={"delete_confirm_title"}
        cancelLabel={"cancel"}
        confirmLabel={"confirm"}
      />
    </div>
  );
};
export default BrandsPage;
