import { Component, OnInit, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { combineLatest } from 'rxjs';
import { takeWhile } from 'rxjs/operators';
import { NbThemeService } from '@nebular/theme';
import { registerMap } from 'echarts';
import { registerShape } from 'echarts';
import { extendShape } from 'echarts';
import { circle } from 'leaflet';

@Component({
  selector: 'ngx-bubble-map',
  styleUrls: ['./bubble-map.component.scss'],
  template: `
    <nb-card>
      <nb-card-header>Bubble Maps</nb-card-header>
      <div echarts [options]="options" class="echarts"></div>
    </nb-card>
  `,
})
export class BubbleMapComponent implements OnInit, OnDestroy {

  latlong: any = {};
  mapData: any[];
  max = -Infinity;
  min = Infinity;
  options: any;

  bubbleTheme: any;
  geoColors: any[];

  d: any;

  private alive = true;

  async createRenderItem() {

    await this.gizmo();

    const d = this.d;
    
    
    return function (params, api) {
      console.log('renderItem params = ', params);
      console.log('renderItem api = ', api);
      console.log('renderItem d = ', d);
      const dCur = d.renderData[params.seriesId][params.dataIndex];
      console.log('renderItem dCur = ', dCur);

      const completeset = [];
      dCur.pointSet.forEach(function (ps) {
        completeset.push(api.coord(ps));
      });

      const ret = {
        type: 'polygon',
        z2: dCur.z,
        name: dCur.name,
        id: dCur.id,
        shape: {},
        silent: true,
        style: api.style(dCur.style)
      };
      if ( true ) {
        ret.shape = {
          points: completeset
        }
      } else {
        ret.shape = {
          points: echarts.graphic.clipPointsByRect(completeset, {
            x: params.coordSys.x,
            y: params.coordSys.y,
            width: params.coordSys.width,
            height: params.coordSys.height
          })
        };
      }
      console.log('renderItem return = ', ret);
      return ret;
    };
  }


  async gizmo() {
    this.http.get('/assets/exampleData1.json')
      .subscribe(data => this.ff(data)
      );
  }

  // async gizmo() {
  //   this.http.get('/assets/exampleData1.json')
  //     .subscribe(data => {
  //       this.d = data;
  //       const renderData = {};
  //       let i = 0;
  //       this.d.diagramData.forEach(function (x) {
  //         Object.entries(x).forEach(([key, array]) => {
  //           const renderDataArray = [];
  //           this.array.forEach(function (y) {
  //             y.z = ++i;
  //             renderDataArray.push(y);
  //           });
  //           renderData[key] = renderDataArray;
  //         });
  //       });
  //       this.d.renderData = renderData;
  //       // this.loadChart();
  //     });
  // }

  ff(data){
    this.d = data;
      const renderData = {};
      let i = 0;
      this.d.diagramData.forEach(function (x) {
        Object.entries(x).forEach(([key, array]) => {
          const renderDataArray = [];
          this.array.forEach(function (y) {
            y.z = ++i;
            renderDataArray.push(y);
          });
          renderData[key] = renderDataArray;
        });
      });
      this.d.renderData = renderData;
  }

  ngOnInit() {
    //this.gizmo();
  }

  constructor(private theme: NbThemeService,
              private http: HttpClient) {

    combineLatest([
      this.http.get('assets/map/world.json'),
      this.theme.getJsTheme(),
    ])
      .pipe(takeWhile(() => this.alive))
      .subscribe(([map, config]: [any, any]) => {

        // registerShape('myCustomShape', extendShape({
        //   shape: {
        //       x: 0,
        //       y: 0,
        //       width: 0,
        //       height: 0
        //   },
        //   buildPath: (ctx, shape) => {
        //       ctx.moveTo(shape.x, shape.y);
        //       ctx.lineTo(shape.x + shape.width, shape.y);
        //       ctx.lineTo(shape.x, shape.y + shape.height);
        //       ctx.lineTo(shape.x + shape.width, shape.y + shape.height);
        //       ctx.closePath();
        //   }
        // }));

        registerMap('world', map);

        const colors = config.variables;
        this.bubbleTheme = config.variables.bubbleMap;
        this.geoColors = [colors.primary, colors.info, colors.success, colors.warning, colors.danger];

        this.mapData = [
          { 'latitude': 23, 'longitude': 55, 'name': 'yyy', 'value': 200, 'color': this.getRandomGeoColor() },
          { 'latitude': 11, 'longitude': 10, 'name': 'xxx', 'value': 100, 'color': this.getRandomGeoColor() }];

        this.mapData.forEach((itemOpt) => {
          if (itemOpt.value > this.max) {
            this.max = itemOpt.value;
          }
          if (itemOpt.value < this.min) {
            this.min = itemOpt.value;
          }
        });

        //this.gizmo();
        const renderItem = this.createRenderItem();

        this.options = {
          
          tooltip: {
            trigger: 'item',
            formatter: params => {
              return `${params.name}: ${params.value[2]}`;
            },
          },
          
          visualMap: {
            show: false,
            min: 0,
            max: this.max,
            inRange: {
              symbolSize: [3, 30],
            },
          },
          
          geo: {
            name: 'World Population (2010)',
            type: 'map',
            map: 'world',
            roam: true,
            label: {
              emphasis: {
                show: false,
              },
            },
            itemStyle: {
              normal: {
                areaColor: this.bubbleTheme.areaColor,
                borderColor: this.bubbleTheme.areaBorderColor,
              },
              emphasis: {
                areaColor: this.bubbleTheme.areaHoverColor,
              },
            },
            zoom: 1.1,
          },
          
          series: [
            {
              type: 'custom',
              // type: 'scatter',
              // type: 'circle',
              coordinateSystem: 'geo',
              // renderItem: 'square',
              renderItem: renderItem,
              // renderItem: (params, api) => {
              //   return {
              //       type: 'myCustomShape',
              //       shape: {
              //           x: api.value(0),
              //           y: api.value(1),
              //           width: api.value(2),
              //           height: api.value(3)
              //       },
              //       style: {
              //           fill: 'red'
              //       }
              //   };
              // },
              data: this.mapData.map(itemOpt => {
                return {
                  name: itemOpt.name,
                  value: [
                    itemOpt.longitude,
                    itemOpt.latitude,
                    itemOpt.value,
                  ],
                  itemStyle: {
                    normal: {
                      color: itemOpt.color,
                    },
                  },
                };
              }),
            },
          ]
          
        };
        
      });
  }

  ngOnDestroy() {
    this.alive = false;
  }

  private getRandomGeoColor() {
    const index = Math.round(Math.random() * this.geoColors.length);
    return this.geoColors[index];
  }
}
