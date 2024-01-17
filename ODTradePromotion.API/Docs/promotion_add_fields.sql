ALTER TABLE public."TpPromotions"
ADD COLUMN "IsSync" boolean DEFAULT false,
ADD COLUMN "OwnerType" character varying(100) NULL,
ADD COLUMN "OwnerCode" character varying(255) NULL,
ADD COLUMN "PrincipalCode" character varying(100) NULL,
ADD COLUMN "DisSicCode" character varying(100) NULL,
ADD COLUMN "DisSicDesc" character varying(500) NULL,
ADD COLUMN "IsFlashSales" boolean DEFAULT false,
ADD COLUMN "FsValidHour" timestamp NULL,
ADD COLUMN "FsUntilHour" timestamp NULL

ALTER TABLE public."TpPromotionDefinitionProductForGifts"
ADD COLUMN "OwnerType" character varying(100) NULL,
ADD COLUMN "OwnerCode" character varying(255) NULL

ALTER TABLE public."TpPromotionDefinitionProductForSales"
ADD COLUMN "OwnerType" character varying(100) NULL,
ADD COLUMN "OwnerCode" character varying(255) NULL

ALTER TABLE public."TpPromotionDefinitionStructures"
ADD COLUMN "OwnerType" character varying(100) NULL,
ADD COLUMN "OwnerCode" character varying(255) NULL

ALTER TABLE public."TpPromotionObjectCustomerAttributeLevels"
ADD COLUMN "OwnerType" character varying(100) NULL,
ADD COLUMN "OwnerCode" character varying(255) NULL

ALTER TABLE public."TpPromotionObjectCustomerAttributeValues"
ADD COLUMN "OwnerType" character varying(100) NULL,
ADD COLUMN "OwnerCode" character varying(255) NULL

ALTER TABLE public."TpPromotionObjectCustomerShiptos"
ADD COLUMN "OwnerType" character varying(100) NULL,
ADD COLUMN "OwnerCode" character varying(255) NULL

ALTER TABLE public."TpPromotionScopeDsas"
ADD COLUMN "OwnerType" character varying(100) NULL,
ADD COLUMN "OwnerCode" character varying(255) NULL

ALTER TABLE public."TpPromotionScopeTerritorys"
ADD COLUMN "OwnerType" character varying(100) NULL,
ADD COLUMN "OwnerCode" character varying(255) NULL