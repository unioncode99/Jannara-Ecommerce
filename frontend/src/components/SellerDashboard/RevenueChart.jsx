import React from "react";
import {
  ResponsiveContainer,
  LineChart,
  Line,
  XAxis,
  YAxis,
  Tooltip,
  CartesianGrid,
} from "recharts";
import { useLanguage } from "../../hooks/useLanguage";
import { useTheme } from "../../hooks/useTheme";

// Month names
const MONTHS = {
  en: [
    "Jan",
    "Feb",
    "Mar",
    "Apr",
    "May",
    "Jun",
    "Jul",
    "Aug",
    "Sep",
    "Oct",
    "Nov",
    "Dec",
  ],
  ar: [
    "يناير",
    "فبراير",
    "مارس",
    "أبريل",
    "مايو",
    "يونيو",
    "يوليو",
    "أغسطس",
    "سبتمبر",
    "أكتوبر",
    "نوفمبر",
    "ديسمبر",
  ],
};

// Format data for chart
const formatData = (data, language = "en") => {
  if (!data || !data.length) return [];
  return data.map((item) => ({
    name: `${MONTHS[language][item.month - 1]} ${item.year}`,
    revenue: Number(item.revenue),
  }));
};

// Custom tooltip
const CustomTooltip = ({ active, payload, label, theme, language }) => {
  if (!active || !payload?.length) return null;

  const bgColor = theme === "dark" ? "#111827" : "#fff";
  const textColor = theme === "dark" ? "#e5e7eb" : "#111827";
  const revenueLabel = language === "ar" ? "الإيرادات" : "Revenue";

  return (
    <div
      style={{
        background: bgColor,
        color: textColor,
        padding: "0.5rem 1rem",
        borderRadius: "0.5rem",
        boxShadow: "0 2px 6px rgba(0,0,0,0.2)",
      }}
    >
      <p>{label}</p>
      <p>{`${revenueLabel}: $${payload[0].value.toLocaleString()}`}</p>
    </div>
  );
};

const RevenueChart = ({ data }) => {
  const { language } = useLanguage();
  const { theme } = useTheme();

  const chartData = formatData(data, language);
  console.log("chartData ->", chartData); // for testing

  const strokeColor = theme === "dark" ? "#818cf8" : "#4f46e5";
  const gridStroke = theme === "dark" ? "#374151" : "#e5e7eb";

  return (
    <div style={{ width: "100%", height: 350 }}>
      <ResponsiveContainer>
        <LineChart data={chartData}>
          <CartesianGrid stroke={gridStroke} strokeDasharray="3 3" />
          <XAxis
            dataKey="name"
            tick={{ fill: theme === "dark" ? "#e5e7eb" : "#111827" }}
          />
          <YAxis
            tickFormatter={(value) => value.toLocaleString()}
            domain={[0, "dataMax + 1000"]}
            tick={{ fill: theme === "dark" ? "#e5e7eb" : "#111827" }}
          />
          <Tooltip
            content={<CustomTooltip theme={theme} language={language} />}
          />
          <Line
            type="monotone"
            dataKey="revenue"
            stroke={strokeColor}
            strokeWidth={3}
            dot={true}
            animationDuration={1200}
          />
        </LineChart>
      </ResponsiveContainer>
    </div>
  );
};

export default RevenueChart;
