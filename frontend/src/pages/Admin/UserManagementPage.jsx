import { useEffect, useState } from "react";
import ViewSwitcher from "../../components/CustomerOrders/ViewSwitcher";
import AddEditProductCategoryModal from "../../components/ProductCategoriesManagementPage/AddEditProductCategoryModal";
import Button from "../../components/ui/Button";
import ConfirmModal from "../../components/ui/ConfirmModal";
import "./UserManagementPage.css";
import "../../hooks/useLanguage";
import { useLanguage } from "../../hooks/useLanguage";
import { Layers, Plus, User, UserPlus } from "lucide-react";
import CardView from "../../components/UserManagementPage.jsx/CardView";
import TableView from "../../components/UserManagementPage.jsx/TableView";
import AddAdminModal from "../../components/UserManagementPage.jsx/AddAdminModal";
import { read } from "../../api/apiWrapper";

const UserManagementPage = () => {
  const [view, setView] = useState("table"); // 'table' or 'card'
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(false);
  const [isAddUserModalOpen, setIsAddUserModalOpen] = useState(false);
  const [isDactivateUserConfirmModalOpen, setIsDactivateUserConfirmModalOpen] =
    useState(false);
  const [selectedUser, setSelectedUser] = useState(null);
  const { translations } = useLanguage();
  const {
    title,
    add_admin,
    confirm,
    cancel,
    delete_confirm_title,
    add_success,
    edit_success,
    delete_success,
    action_failed,
    deactivate_user,
    activate_user,
    confirm_deactivate,
    confirm_activate,
  } = translations.general.pages.users_management;

  const fetchUsers = async () => {
    try {
      setLoading(true);
      const data = await read(`users?pageNumber=1&pageSize=10`); // for test
      setUsers(data?.items);
      console.log("data", data);
    } catch (error) {
      console.error("Failed to fetch categories:", error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchUsers();
  }, []);

  function handeAddAdmin() {}

  function handleToggleUserStatus(user) {
    console.log("user -> ", user);
    setSelectedUser(user);
    setIsDactivateUserConfirmModalOpen(true);
  }

  const closeModal = () => {
    setIsAddUserModalOpen(false);
    setIsDactivateUserConfirmModalOpen(false);
    setSelectedUser(null);
  };

  async function AddAdmin() {}

  async function DactivateUser() {}

  return (
    <div className="product-categori-management-container">
      <header>
        <h1>
          <User /> {title}
        </h1>
        <Button onClick={handeAddAdmin} className="btn btn-primary">
          <UserPlus /> {add_admin}
        </Button>
      </header>
      <ViewSwitcher view={view} setView={setView} />
      {view == "card" && (
        <CardView
          users={users}
          handleToggleUserStatus={handleToggleUserStatus}
        />
      )}
      {view == "table" && (
        <TableView
          users={users}
          handleToggleUserStatus={handleToggleUserStatus}
        />
      )}
      <AddAdminModal
        show={isAddUserModalOpen}
        onClose={closeModal}
        onConfirm={AddAdmin}
      />
      <ConfirmModal
        show={isDactivateUserConfirmModalOpen}
        onClose={() => closeModal()}
        onConfirm={() => DactivateUser()}
        title={
          selectedUser?.roles[0]?.isActive
            ? confirm_deactivate
            : confirm_activate
        }
        cancelLabel={cancel}
        confirmLabel={
          selectedUser?.roles[0]?.isActive ? deactivate_user : activate_user
        }
      />
    </div>
  );
};
export default UserManagementPage;
