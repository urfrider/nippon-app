import React from "react";
import ApexChart from "react-apexcharts";
import { ChartContainer, ChartTitle } from "./Styled-Containers";

function BarChart({ title, labels, data }) {
  return (
    <ChartContainer>
      <ChartTitle>
        <h1>{title}</h1>
      </ChartTitle>
      <ApexChart
        type="bar"
        height="90%"
        width="100%"
        series={[
          {
            data: data,
          },
        ]}
        options={{
          bar: {
            columnWidth: "45%",
            distributed: true,
          },
          xaxis: {
            categories: labels,
          },
          labels: {
            style: {
              fontSize: "12px",
            },
          },
          colors: ["#ff9f1a"],
        }}
      />
    </ChartContainer>
  );
}

export default BarChart;
