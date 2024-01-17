ALTER TABLE public."TpDiscounts"
ADD COLUMN "OwnerType" character varying(100) NULL,
ADD COLUMN "OwnerCode" character varying(255) NULL,
ADD COLUMN "DisSicCode" character varying(10) NULL;

ALTER TABLE public."TpDiscountScopeTerritorys"
ADD COLUMN "OwnerType" character varying(100) NULL,
ADD COLUMN "OwnerCode" character varying(255) NULL;

ALTER TABLE public."TpDiscountScopeDsas"
ADD COLUMN "OwnerType" character varying(100) NULL,
ADD COLUMN "OwnerCode" character varying(255) NULL;

ALTER TABLE public."TpDiscountObjectCustomerAttributeLevels"
ADD COLUMN "OwnerType" character varying(100) NULL,
ADD COLUMN "OwnerCode" character varying(255) NULL;

ALTER TABLE public."TpDiscountObjectCustomerAttributeValues"
ADD COLUMN "OwnerType" character varying(100) NULL,
ADD COLUMN "OwnerCode" character varying(255) NULL;

ALTER TABLE public."TpDiscountObjectCustomerShiptos"
ADD COLUMN "OwnerType" character varying(100) NULL,
ADD COLUMN "OwnerCode" character varying(255) NULL;

ALTER TABLE public."TpDiscountStructureDetails"
ADD COLUMN "OwnerType" character varying(100) NULL,
ADD COLUMN "OwnerCode" character varying(255) NULL;