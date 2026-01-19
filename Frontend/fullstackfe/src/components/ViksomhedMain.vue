<template>
  <v-container class="fill-height d-flex align-center" max-width="900">
    <v-row>
        <v-col cols="4">
            <h2>Virksomheder</h2>      
        </v-col>
        <v-col cols="4">            
            <v-btn
                variant="text"
                icon
                @click="create()"
            >                
                <v-icon>mdi-office-building-plus</v-icon>
                Opret ny
            </v-btn>
        </v-col>
        <v-col cols="4"><v-alert text="Ej færdig kodet" type="warning" closable v-model="ejkodet"> Ny cvr: {{ newCvr }}</v-alert></v-col>
        <v-col cols="12">
            <v-expansion-panels>
                <v-expansion-panel
                    v-for="(virksomhed, index) in items"
                    :key="virksomhed.cvr"
                >
                    <!-- Panel-header med titel -->
                    <v-expansion-panel-title>
                        <h3>{{ virksomhed.navn }}</h3>
                    </v-expansion-panel-title>

                    <!-- Panel-indhold -->
                    <v-expansion-panel-text>                        
                        <p>
                            <i>Cvr-nr:</i> {{ virksomhed.cvr }}
                            <br/>                        
                            <i>Adresse:</i> {{ virksomhed.adresse }}, {{ virksomhed.postnr }} {{ virksomhed.by }}
                            <v-btn
                                variant="text"
                                icon
                                @click="edit(virksomhed)"
                                @click.stop
                            >
                                <v-icon>mdi-pencil</v-icon>
                            </v-btn>

                            <v-btn 
                                variant="text"
                                icon
                                @click="remove(virksomhed.cvr)"
                                @click.stop
                            >
                                <v-icon>mdi-delete</v-icon>
                            </v-btn>
                        </p>
                    </v-expansion-panel-text>
                </v-expansion-panel>
            </v-expansion-panels>
        </v-col>
    </v-row>
  </v-container>

  <v-dialog v-model="editDialog" max-width="500">
    <template v-slot:activator="{ props }"></template>
    <v-confirm-edit
      ref="confirm"
      v-model="model"
      ok-text="Gem"
      cancel-text="Fortryd"
      @cancel="editDialog = false"
      @save="save"
    >
      <template v-slot:default="{ model: proxyModel, actions }">
        <v-card title="Rediger virksomhed">
          <v-card-text>
            <v-text-field
              v-model="proxyModel.value.navn"
              label="Ret navn"
            ></v-text-field>

            <v-text-field
              v-model="proxyModel.value.adresse"
              label="Ret adresse"
            ></v-text-field>
          </v-card-text>

          <template v-slot:actions>
            <v-spacer></v-spacer>
            <component :is="actions"></component>
          </template>
        </v-card>
      </template>
    </v-confirm-edit>
  </v-dialog>

  <v-dialog v-model="createDialog" max-width="500">    
    <template v-slot:activator="{ props }"></template>
    <v-confirm-edit
      ref="confirm"
      v-model="newCvr"
      ok-text="Opret"
      cancel-text="Fortryd"
      @cancel="createDialog = false"
      @save="getNew"
    >
      <template v-slot:default="{ model: proxyCvr, actions }">
        <v-card title="Opret virksomhed">
          <v-card-text>
            <v-text-field
              v-model="proxyCvr.value"
              label="CVR"
            ></v-text-field>
          </v-card-text>

          <template v-slot:actions>
            <v-spacer></v-spacer>
            <component :is="actions"></component>
          </template>
        </v-card>
      </template>
    </v-confirm-edit>
  </v-dialog>


</template>

<script lang="ts">
import { defineComponent } from 'vue';

interface Virksomhed {
    cvr: number;
    navn: string;
    adresse: string;
    postnr: number;
    by: string;
}

interface Model {
    navn: string,
    adresse: string
}

export default defineComponent({ 
    data() {
        return {
            editDialog: false,
            createDialog: false,
            ejkodet: false,
            selected: null as number | null,
            confirm: null,
            newCvr: null as number | null,
            // model til redigering
            model: {
                navn: '',
                adresse: '',
            } as Model, 
            // Data: Et array af virksomheder (der skal komme fra graphql ???)
            items: [
                { cvr: 42954616, navn: 'STILLING KØL & EL ApS', adresse: 'Niels Bohrs Vej 15A', postnr: 8660 , by: 'Skanderborg'},
                { cvr: 17477994, navn: 'Risskov El & VVS & Ventilation A/S', adresse: 'Ved Skoven 45, 1', postnr: 8541 , by: 'Skødstrup'},
                { cvr: 28106661, navn: 'Skødstrup Tandklinik ApS.', adresse: 'Grenåvej 728', postnr: 8541 , by: 'Skødstrup'},
                { cvr: 51261040, navn: 'TeamKey ApS', adresse: 'Søndersøparken 19F, 1. 3.', postnr: 8800 , by: 'Viborg'},
            ] as Virksomhed[],
        };
    },
    methods: {
        create () : void {
            this.newCvr = 0;
            this.createDialog = true;
        },
        getNew () : void {
            this.createDialog = false;            
            this.ejkodet = true;
        },
        edit (item: Virksomhed) : void {
            this.selected = item.cvr;
            this.model = { navn: item.navn, adresse: item.adresse };
            this.editDialog = true;
        },
        save() : void { // Update item data
            this.editDialog = false

            this.items = this.items.map(item =>
                item.cvr === this.selected
                    ? { ...item, navn: this.model.navn, adresse: this.model.adresse }
                    : item
            )
        },
        remove (cvr: number) {
            this.items = this.items.filter( (item) => item.cvr !== cvr)
        },
    },
});
</script>