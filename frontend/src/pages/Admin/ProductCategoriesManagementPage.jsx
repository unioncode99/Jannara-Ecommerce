import { Layers, Plus } from "lucide-react";
import Button from "../../components/ui/Button";
import "./ProductCategoriesManagementPage.css";
import CardView from "../../components/ProductCategoriesManagementPage/CardView";
import ViewSwitcher from "../../components/CustomerOrders/ViewSwitcher";
import { useEffect, useState } from "react";
import TableView from "../../components/ProductCategoriesManagementPage/TableView";
import { create, read, remove, update } from "../../api/apiWrapper";
import { useLanguage } from "../../hooks/useLanguage";
import SpinnerLoader from "../../components/ui/SpinnerLoader";
import AddEditProductCategoryModal from "../../components/ProductCategoriesManagementPage/AddEditProductCategoryModal";
import ConfirmModal from "../../components/ui/ConfirmModal";
import { toast } from "../../components/ui/Toast";

const ProductCategoriesManagementPage = () => {
  const [view, setView] = useState("table"); // 'table' or 'card'
  const [productCategories, setProductCategories] = useState([]);
  const [loading, setLoading] = useState(false);
  const [
    isAddEditProductCategoryModalOpen,
    setIsAddEditProductCategoryModalOpen,
  ] = useState(false);
  const [
    isDeleteProductCategoryConfirmModalOpen,
    setIsDeleteProductCategoryConfirmModalOpen,
  ] = useState(false);
  const [selectedProductCategory, setSelectedProductCategory] = useState(null);
  const { translations } = useLanguage();

  const {
    title,
    confirm,
    cancel,
    add_category,
    delete_confirm_title,
    add_success,
    edit_success,
    delete_success,
    action_failed,
  } = translations.general.pages.product_categories_management;

  const fetchCategories = async () => {
    try {
      setLoading(true);
      const data = await read("product-categories");
      setProductCategories(data);
      console.log("data", data);
    } catch (error) {
      console.error("Failed to fetch categories:", error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchCategories();
  }, []);

  function handleDeleteProductCategory(productCategory) {
    setSelectedProductCategory(productCategory);
    setIsDeleteProductCategoryConfirmModalOpen(true);
    // console.log("productCategory -> ", productCategory);
  }
  function handleEditProductCategory(productCategory) {
    setSelectedProductCategory(productCategory);
    setIsAddEditProductCategoryModalOpen(true);
    // console.log("productCategory -> ", productCategory);
  }

  function handleAddProductCategory() {
    setSelectedProductCategory(null);
    setIsAddEditProductCategoryModalOpen(true);
    // console.log("productCategory -> ", productCategory);
  }

  const closeModal = () => {
    setIsAddEditProductCategoryModalOpen(false);
    setIsDeleteProductCategoryConfirmModalOpen(false);
    setSelectedProductCategory(null);
  };

  async function addEditProductCategory(payload) {
    // e.preventDefault();
    let result = null;
    try {
      if (selectedProductCategory) {
        result = await update(
          `product-categories/${selectedProductCategory.id}`,
          payload
        );
        toast.show(edit_success, "success");
      } else {
        result = await create(`product-categories`, payload);
        toast.show(add_success, "success");
      }

      console.log("result -> ", result);
      setProductCategories((prev) => {
        const exists = prev.some((cat) => cat.id === result.id);

        if (exists) {
          return prev.map((cat) => (cat.id === result.id ? result : cat));
        }

        return [...prev, result];
      });
    } catch (error) {
      console.log("addEditProductCategory -> ", error);
      toast.show(action_failed, "error");
    } finally {
      closeModal();
    }
  }

  async function deleteProductCategory() {
    try {
      await remove(`product-categories/${selectedProductCategory.id}`);
      toast.show(delete_success, "success");
      setProductCategories((prev) =>
        prev.filter((cat) => cat.id != selectedProductCategory.id)
      );
    } catch (error) {
      console.log("error -> ", error);
      toast.show(action_failed, "error");
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
    <div className="product-categori-management-container">
      <header>
        <h1>
          <Layers /> {title}
        </h1>
        <Button onClick={handleAddProductCategory} className="btn btn-primary">
          <Plus /> {add_category}
        </Button>
      </header>
      <ViewSwitcher view={view} setView={setView} />
      {view == "card" && (
        <CardView
          productCategories={productCategories}
          handleDeleteProductCategory={handleDeleteProductCategory}
          handleEditProductCategory={handleEditProductCategory}
        />
      )}
      {view == "table" && (
        <TableView
          productCategories={productCategories}
          handleDeleteProductCategory={handleDeleteProductCategory}
          handleEditProductCategory={handleEditProductCategory}
        />
      )}
      <AddEditProductCategoryModal
        show={isAddEditProductCategoryModalOpen}
        onClose={closeModal}
        onConfirm={addEditProductCategory}
        productCategory={selectedProductCategory}
        productCategories={productCategories}
      />
      <ConfirmModal
        show={isDeleteProductCategoryConfirmModalOpen}
        onClose={() => closeModal()}
        onConfirm={() => deleteProductCategory()}
        title={delete_confirm_title}
        cancelLabel={cancel}
        confirmLabel={confirm}
      />
    </div>
  );
};
export default ProductCategoriesManagementPage;
