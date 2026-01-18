<template>
  <v-container class="fill-height d-flex align-center" max-width="900">
    <v-row>
        <v-col cols="12">
            <h1 class="text-h2 my-0 font-weight-bold">Virksomheder</h1>
        </v-col>

        <v-col cols="12">
            HER TIL
            <!-- Brug eksempel fra: 
             https://vuetifyjs.com/en/components/dialogs/#data-table 
             -->
            <v-expansion-panels>
                <v-expansion-panel
                    v-for="(virsksomhed, index) in items"
                    :key="index"
                >
                    <!-- Panel-header med titel -->
                    <v-expansion-panel-title>
                        <h3>{{ virsksomhed.navn }}</h3>
                    </v-expansion-panel-title>

                    <!-- Panel-indhold -->
                    <v-expansion-panel-text>
                        <p><i>Cvr-nr:</i> {{ virsksomhed.cvr }}</p>
                        <p><i>Adresse:</i> {{ virsksomhed.adresse }}, {{ virsksomhed.postnr }} {{ virsksomhed.by }}</p>

                    </v-expansion-panel-text>
                </v-expansion-panel>
            </v-expansion-panels>
        </v-col>
    </v-row>

  </v-container>

</template>

<script lang="ts">
    import { defineComponent } from 'vue';

    // Definer typen for hvert panel
    interface Virksomhed {
        cvr: number;
        navn: string;
        adresse: string;
        postnr: number;
        by: string;
    }

    export default defineComponent( { 
        name: 'VirksomhedMain',

        // Data: Et array af virksomheder (der skal komme fra graphql ???)
         data() {
             return {
              items: [
                { cvr: 42954616, navn: 'STILLING KØL & EL ApS', adresse: 'Niels Bohrs Vej 15A', postnr: 8660 , by: 'Skanderborg'},
                { cvr: 17477994, navn: 'Risskov El & VVS & Ventilation A/S', adresse: 'Ved Skoven 45, 1', postnr: 8541 , by: 'Skødstrup'},
                { cvr: 28106661, navn: 'Skødstrup Tandklinik ApS.', adresse: 'Grenåvej 728', postnr: 8541 , by: 'Skødstrup'},
                ] as Virksomhed[],
             };
         },
     });

</script>