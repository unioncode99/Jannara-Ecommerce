import React from "react";
import "./Table.css"; // Import the CSS file
import { useLanguage } from "../../hooks/useLanguage";

const Table = ({ headers, data }) => {
  const { language } = useLanguage(); // Access translations and language context

  return (
    <div className="table-responsive">
      {/* <table className="table table-striped table-hover"> */}
      <table className="table table-hover table-striped">
        <thead>
          <tr>
            {headers.map((header, idx) => (
              <th
                style={{
                  textAlign: language == "en" ? "left" : "right",
                  direction: language == "en" ? "ltr" : "rtl",
                }}
                key={idx}
              >
                {header}
              </th>
            ))}
          </tr>
        </thead>
        <tbody>
          {data.length === 0 ? (
            <tr>
              <td colSpan={headers.length} className="text-center">
                No Data Found
              </td>
            </tr>
          ) : (
            data.map((row, idx) => (
              <tr key={idx}>
                {Object.values(row).map((value, i) => (
                  <td
                    style={{
                      textAlign: language == "en" ? "left" : "right",
                      direction: language == "en" ? "ltr" : "rtl",
                    }}
                    key={i}
                  >
                    {value}
                  </td>
                ))}
              </tr>
            ))
          )}
        </tbody>
      </table>
    </div>
  );
};

export default Table;
