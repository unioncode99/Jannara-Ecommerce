import React from "react";
import "./Pagination.css";
import { ChevronLeft, ChevronRight } from "lucide-react";
import { useLanguage } from "../../hooks/useLanguage";

const Pagination = ({
  currentPage,
  totalItems,
  onPageChange,
  pageSize = 10,
}) => {
  const totalPages = Math.ceil(totalItems / pageSize);
  const { language } = useLanguage();

  // Handle page click
  const handlePageClick = (page) => {
    if (page >= 1 && page <= totalPages) {
      onPageChange(page);
    }
  };

  // Generate range of page numbers (For simplicity, a range of 5 pages)
  const generatePageNumbers = () => {
    let start = Math.max(currentPage - 2, 1); // Start from the 2 pages before the current page
    let end = Math.min(start + 4, totalPages); // End at 5 pages ahead of the start, or the total number of pages

    const pageNumbers = [];
    for (let i = start; i <= end; i++) {
      pageNumbers.push(i);
    }
    return pageNumbers;
  };

  return (
    <div className="pagination scrollable">
      {/* Previous Button */}
      <button
        className="pagination-button"
        onClick={() => handlePageClick(currentPage - 1)}
        disabled={currentPage === 1}
      >
        {language == "en" ? <ChevronLeft /> : <ChevronRight />}
        <span>{language == "en" ? "Prev" : "السابق"}</span>{" "}
      </button>

      {/* Page Number Buttons */}
      {generatePageNumbers().map((page) => (
        <button
          key={page}
          className={`pagination-button ${
            currentPage === page ? "active" : ""
          }`}
          onClick={() => handlePageClick(page)}
        >
          {page}
        </button>
      ))}

      {/* Next Button */}
      <button
        className="pagination-button"
        onClick={() => handlePageClick(currentPage + 1)}
        disabled={currentPage === totalPages}
      >
        {/* Next &gt; */}
        <span>{language == "en" ? "Next" : "التالي"}</span>{" "}
        {language == "en" ? <ChevronRight /> : <ChevronLeft />}
      </button>
    </div>
  );
};

export default Pagination;
