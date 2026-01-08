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

// Month names in English and Arabic
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

// Format chart data based on language
const formatData = (data, language = "en") =>
  data?.map((item) => ({
    name: `${MONTHS[language][item.month - 1]} ${item.year}`,
    revenue: Math.round(item.revenue),
  }));

// Tooltip component with language support
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
  const { language, translations } = useLanguage();
  const { theme } = useTheme();

  const chartData = formatData(data, language);

  const strokeColor = theme === "dark" ? "#818cf8" : "#4f46e5";
  const gridStroke = theme === "dark" ? "#374151" : "#e5e7eb";

  const { platform_growth } = translations.general.pages.admin_dashboard;

  return (
    <>
      <h3>{platform_growth}</h3>
      <ResponsiveContainer width="100%" height={300}>
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
            dot={false}
            animationDuration={1200}
          />
        </LineChart>
      </ResponsiveContainer>
    </>
  );
};

export default RevenueChart;
