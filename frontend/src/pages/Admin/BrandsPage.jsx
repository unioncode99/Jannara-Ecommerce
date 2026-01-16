import { useEffect, useState } from "react";
import { useLanguage } from "../../hooks/useLanguage";
import { read } from "../../api/apiWrapper";
import SpinnerLoader from "../../components/ui/SpinnerLoader";
import { Plus, Tags } from "lucide-react";
import Button from "../../components/ui/Button";
import ViewSwitcher from "../../components/CustomerOrders/ViewSwitcher";
import CardView from "../../components/BrandsPage/CardView";
import TableView from "../../components/BrandsPage/TableView";
import AddEditBrandModal from "../../components/BrandsPage/AddEditBrandModal";
import ConfirmModal from "../../components/ui/ConfirmModal";
import "./BrandsPage.css";

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

  async function addEditBrand(payload) {}
  async function deleteBrand() {}

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
        confirmLabel={confirm}
      />
    </div>
  );
};
export default BrandsPage;
