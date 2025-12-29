import { Grid2x2, TextAlignJustify } from "lucide-react";
import Button from "../ui/Button";
import "./ViewSwitcher.css";

const ViewSwitcher = ({ view, setView }) => {
  return (
    <div className="view-switcher-container">
      <Button
        onClick={() => setView("table")}
        title="Table View"
        className={`view-btn ${view == "table" ? "active" : ""}`}
      >
        <TextAlignJustify />
      </Button>
      <Button
        onClick={() => setView("card")}
        title="Table View"
        className={`view-btn ${view == "card" ? "active" : ""}`}
      >
        <Grid2x2 />
      </Button>
    </div>
  );
};
export default ViewSwitcher;
