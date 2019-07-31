INSERT INTO seg_Usuario_x_Empresa
SELECT 'admin',1,''

insert into seg_Menu_x_Empresa
select 1,IdMenu,'' from seg_Menu

insert into seg_Usuario_x_Empresa
select 'admin',1,''

INSERT INTO [dbo].[tb_sucursal]
           ([IdEmpresa]
           ,[IdSucursal]
           ,[codigo]
           ,[Su_Descripcion]
           ,[Su_CodigoEstablecimiento]
           ,[Su_Ruc]
           ,[Su_JefeSucursal]
           ,[Su_Telefonos]
           ,[Su_Direccion]
           ,[Estado]
           ,[Es_establecimiento]
           ,[IdCtaCble_cxp]
           ,[IdCtaCble_vtaIVA0]
           ,[IdCtaCble_vtaIVA])
select 1,1,'001','MATRIZ','001',NULL,NULL,NULL,NULL,'A',1,NULL,NULL,NULL

insert into seg_usuario_x_tb_sucursal
select 'admin',1,1,''

insert into seg_Menu_x_Empresa_x_Usuario
SELECT 1,'admin',IdMenu,0,0,0 FROM seg_Menu

INSERT INTO [dbo].[tb_bodega]
           ([IdEmpresa]
           ,[IdSucursal]
           ,[IdBodega]
           ,[cod_bodega]
           ,[bo_Descripcion]
           ,[cod_punto_emision]
           ,[bo_manejaFacturacion]
           ,[bo_EsBodega]
           ,[Estado]
           ,[IdEstadoAproba_x_Ing_Egr_Inven]
           ,[IdCentroCosto]
           ,[IdCtaCtble_Inve]
           ,[IdCtaCtble_Costo])
select 1,1,1,'001','MATRIZ','001',1,1,'A','APRO',NULL,NULL,NULL