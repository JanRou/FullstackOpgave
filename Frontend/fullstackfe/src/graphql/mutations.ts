import gql from 'graphql-tag';

export const OPRET_VIRKSOMHED = gql`
  mutation OpretVirksomhed($cvr: Int!) {
    opretVirksomhed(cvr: $cvr) {
      virksomhed {
        cvr
        navn
        adresse
        postnummer
        by
      }
    }
  }
`;

export const OPDATER_VIRKSOMHED = gql`
  mutation OpdaterVirksomhed($input: VirksomhedInTypeInput!) {
    opdaterVirksomhed(input: $input) {
      virksomhed {
        cvr
        navn
        adresse
        postnummer
        by
      }
    }
  }
`;

export const SLET_VIRKSOMHED = gql`
  mutation SletVirksomhed($cvr: Int!) {
    sletVirksomhed(cvr: $cvr)
  }
`;
