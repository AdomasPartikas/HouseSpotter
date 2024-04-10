import React, { useEffect, useState } from "react";
import Header from "../components/header";
import ProductCard from "../components/productCard";

function Home() {
  const [formData, setFormData] = useState({
    objectType: "0",
    municipality: "0",
    settlement: "",
    microdistrict: [],
    street: [],
    areaFrom: "",
    areaTo: "",
    roomsFrom: "",
    roomsTo: "",
    equipment: "",
    priceFrom: "",
    priceTo: "",
    floorFrom: "",
    floorTo: "",
    yearFrom: "",
    yearTo: "",
    heating: "",
    buildingType: "",
    priceForAreaFrom: "",
    priceForAreaTo: "",
    openDoor: false,
    privateSeller: false,
    agencySeller: false,
    newProjects: false,
    sorting: "0",
    date: "",
  });

  const [housingData, setHousingData] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await fetch("/housespotter/db/getallhousing");
        if (!response.ok) throw new Error("Data could not be fetched");
        let data = await response.json();
        
        setHousingData(data);
      } catch (error) {
        console.error("Fetching error:", error);
      }
    };

    fetchData();
  }, [formData]);

  return (
    <div className="home">
      <Header />
      <div className="products">
        <div className="layout">
          <h2>
            Skelbimai <span>({housingData.length})</span>
          </h2>
          {housingData.map((product) => (
            <ProductCard key={product.id} product={product} />
          ))}
        </div>
      </div>
    </div>
  );
}

export default Home;
