import React, { useEffect, useState } from "react";
import { useAuth } from "../contexts/AuthContext";
import Header from "../components/header";
import ProductCard from "../components/productCard";

function Favorite() {
  const { user, refreshFavoriteHousing } = useAuth();
  const [favoritesData, setFavoritesData] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await fetch(
          `/housespotter/db/user/${user.id}/savedSearches`
        );
        if (!response.ok) throw new Error("Data could not be fetched");
        const data = await response.json();
        setFavoritesData(data);
      } catch (error) {
        console.error("Fetching error:", error);
      }
    };

    fetchData();
  }, [refreshFavoriteHousing]);

  return (
    <div className="favorite">
      <Header />
      <div className="products">
        <div className="layout">
          <h2>
            Mėgstamiausi <span>({favoritesData.length})</span>
          </h2>
          {favoritesData.map((product) => (
            <ProductCard key={product.id} product={product} />
          ))}
        </div>
      </div>
    </div>
  );
}

export default Favorite;
