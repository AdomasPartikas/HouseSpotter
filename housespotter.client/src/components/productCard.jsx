import React, { useState, useEffect } from 'react';
import { Link } from "react-router-dom";
import { useAuth } from "../contexts/AuthContext";
import { useNotification } from "../contexts/NotificationContext";

function ProductCard({ product }) {
  const { user, refreshFavorites } = useAuth();
  const { notify } = useNotification();

  const [isInSavedSearches, setIsInSavedSearches] = useState(false);

  useEffect(() => {
    const fetchSavedSearches = async () => {
      if (!user) return;

      try {
        const response = await fetch(
          `housespotter/db/user/${user.id}/savedSearches`,
          {
            headers: {
              "Content-Type": "application/json",
            },
          }
        );

        if (!response.ok) {
          throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const savedSearches = await response.json();

        const isSaved = savedSearches.some(
          (savedSearch) => savedSearch.id === product.id
        );
        setIsInSavedSearches(isSaved);
      } catch (error) {
        console.error("Failed to fetch saved searches:", error);
        notify("Nepavyko užkrauti išsaugotų paieškų.", "error");
      }
    };

    fetchSavedSearches();
  }, [user, product.id, notify]);

  const handleFavoriteButton = async () => {
    if (!user) {
      notify("Norint pridėti prie mėgstamiausių, prisijunkite.", "error");
      return;
    }

    const apiUrl = `housespotter/db/user/${user.id}/${
      isInSavedSearches ? "removeSearch" : "saveSearch"
    }`;

    try {
      const response = await fetch(apiUrl, {
        method: isInSavedSearches ? "DELETE" : "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(product.id),
      });

      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }

      setIsInSavedSearches(!isInSavedSearches);
      notify(
        `Skelbimas ${
          isInSavedSearches ? "pašalintas iš" : "pridėtas prie"
        } mėgstamiausių.`,
        "success"
      );
      refreshFavorites();
    } catch (error) {
      notify("Veiksmo atlikti nepavyko.", "error");
      console.error("Favorite action failed:", error);
    }
  };

  return (
    <div className="product">
      <div className="product__photo"></div>
      <div className="product__content">
        <Link to={`/skelbimas/${product.id}`} className="product__title">
          {product.title ? product.title : "Neskelbiama"}
        </Link>
        <button
          className={`product__favorite ${isInSavedSearches ? "active" : ""}`}
          onClick={handleFavoriteButton}
        ></button>
        <div className="product__row">
          <div className="product__price">
            <p>
              {product.kaina
                ? product.kaina
                : product.kainaMen
                ? product.kainaMen
                : "Neskelbiama"}{" "}
              €
            </p>
            <p>
              {product.kaina
                ? (product.kaina / product.plotas).toFixed(2)
                : product.kainaMen
                ? (product.kainaMen / product.plotas).toFixed(2)
                : "Neskelbiama"}{" "}
              €/m2
            </p>
          </div>
          <div className="product__props">
            <div className="product__rooms">
              <p>
                Kambariai:{" "}
                <span>
                  {product.kambariuSk ? product.kambariuSk : "Neskelbiama"}
                </span>
              </p>
            </div>
            <div className="product__area">
              <p>
                Plotas:{" "}
                <span>{product.plotas ? product.plotas : "Neskelbiama"}</span>
              </p>
            </div>
            <div className="product__floors">
              <p>
                Aukštai:{" "}
                <span>
                  {product.aukstai && product.aukstuSk
                    ? product.aukstai + " / " + product.aukstuSk
                    : product.aukstuSk
                    ? product.aukstuSk
                    : "Neskelbiama"}
                </span>
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default ProductCard;