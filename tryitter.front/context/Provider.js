import PropTypes from 'prop-types';
import React, { useMemo, useState } from 'react';
import Context from './Context';

export function Provider({ children }) {
  const [buyProducts, setBuyProducts] = useState([]);
  const [saleId, setSaleId] = useState([]);

  const context = useMemo(
    () => ({
      buyProducts,
      setBuyProducts,
      saleId,
      setSaleId,
    }),
    [buyProducts, saleId],
  );

  return <Context.Provider value={ context }>{children}</Context.Provider>;
}

Provider.propTypes = {
  children: PropTypes.node.isRequired,
};

export default Provider;
