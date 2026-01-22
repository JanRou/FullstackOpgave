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
            <div v-if="$apollo.queries.items.loading">Indlæser virksomheder...</div>
            <v-expansion-panels v-else>
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
                            <i>Adresse:</i> {{ virksomhed.adresse }}, {{ virksomhed.postnummer }} {{ virksomhed.by }}
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

            <v-text-field
              v-model.number="proxyModel.value.postnummer"
              label="Ret postnummer"
            ></v-text-field>

            <v-text-field
              v-model="proxyModel.value.by"
              label="Ret by"
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
              v-model.number="proxyCvr.value"
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
import { HENT_ALLE_VIRKSOMHEDER } from '@/graphql/queries';
import { OPRET_VIRKSOMHED, OPDATER_VIRKSOMHED, SLET_VIRKSOMHED } from '@/graphql/mutations';
import type { Virksomhed, VirksomhedInTypeInput } from '@/graphql/types';

interface Model {
    navn: string,
    adresse: string
    postnummer: number
    by: string
}

export default defineComponent({
    apollo: {
        items: {
            query:  HENT_ALLE_VIRKSOMHEDER,
            update(data) {
                return data.hentAlleVirksomheder.map((v: any) => ({
                    cvr: v.cvr, navn: v.navn, adresse: v.adresse, postnummer: v.postnummer, by: v.by,
                }));
            },
        }
    },
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
            items: [] as Virksomhed[],
        };
    },
    methods: {
        async create () : Promise<void> {
            this.newCvr = 0;
            this.createDialog = true;
        },
        async getNew () : Promise<void> {
            if (this.newCvr !== 0) {
                try {
                    await this.$apollo.mutate({
                        mutation: OPRET_VIRKSOMHED,
                        variables: { cvr: Number(this.newCvr) },
                        update: (cache, { data: { opretVirksomhed } }) => {
                            this.items.push(opretVirksomhed.virksomhed);
                        },
                    });
                } catch (error) {
                     console.error("Fejl ved oprettelse af virksomhed:", error);
                }
                // Hmm Luk dialogen selvom det fejlede, eller er det bedre at lade den stå?
                this.createDialog = false;
            }
        },
        async edit (item: Virksomhed) : Promise<void> {
            this.selected = item.cvr;
            this.model = { navn: item.navn, adresse: item.adresse, postnummer: item.postnummer, by: item.by };
            this.editDialog = true;
        },
        async save() : Promise<void> { // Update item data
            if (this.selected) {
                try {
                    const input: VirksomhedInTypeInput = {
                        cvr: Number(this.selected), // skal ikke ændres
                        navn: this.model.navn,
                        adresse: this.model.adresse,
                        postnummer: Number(this.model.postnummer),
                        by: this.model.by,
                    };

                    await this.$apollo.mutate({
                        mutation: OPDATER_VIRKSOMHED,
                        variables: {
                            input,
                        },
                        update: (cache, { data: { opdaterVirksomhed } }) => {
                            const index = this.items.findIndex((v) => v.cvr === this.selected);
                            if (index !== -1) {
                                this.items.splice(index, 1, opdaterVirksomhed.virksomhed);
                            }
                        },
                    });
                } catch (error) {
                    console.error("Fejl ved opdatering af virksomhed:", error);
                }
                this.editDialog = false;
            }                        
        },
        async remove (cvr: number) : Promise<void> {
            try {
                await this.$apollo.mutate({
                    mutation: SLET_VIRKSOMHED,
                    variables: { cvr: Number(cvr) },
                    update: (cache ) => {
                        this.items = this.items.filter((item) => item.cvr !== cvr);                    },
                });
            } catch(error) {
                console.error("Fejl ved sletning af virksomhed:", error); 
            }
        },
    },
});
</script>