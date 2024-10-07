import React from 'react';
import '../assets/styles/ProductCard.scss';


function ProductCard({ product }) {
    return (
        <div className="product">
          <div className="product__photo"></div>
          <div className="product__content">
            <div className="product__title">
            {product.title ? product.title : "Neskelbiama"}
            </div>
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
