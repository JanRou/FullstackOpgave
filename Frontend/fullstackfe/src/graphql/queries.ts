import gql from 'graphql-tag';

export const HENT_ALLE_VIRKSOMHEDER = gql`
  query HentAlleVirksomheder {
    hentAlleVirksomheder {
      cvr
      navn
      adresse
      postnummer
      by
    }
  }
`;
